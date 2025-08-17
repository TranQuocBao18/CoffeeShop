using CoffeeShop.Shared.Auth;
using CoffeeShop.Shared.Types;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoffeeShop.Shared.Attributes
{
    internal class PermissionAuth
    {
        public string Key { get; }
        public string Permission { get; }

        public PermissionAuth(string key, string permission)
        {
            Key = key;
            Permission = permission;
        }
    }

    public class HasPermissionFunctionsAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        private readonly IDictionary<string, PermissionAuth> _permissions;
        private const string MASTER_PERMISSION = "0.f";
        private const string PERMISSION_FULL = "f";

        public HasPermissionFunctionsAttribute(params string[] permissions)
        {
            _permissions = AuthExtensions.GetPermissionMatrix(permissions).Select(x =>
            {
                var permissonParsed = x.Split('.');
                return new PermissionAuth(permissonParsed[0], permissonParsed[1]);
            }).ToDictionary(x => x.Key);
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userPermissionClaims = context.HttpContext.User.FindAll(Permissions.PERMISSION_CLAIM_TYPE).ToList();

            if (userPermissionClaims.Count != 0 && userPermissionClaims.Exists(p => p.Value == MASTER_PERMISSION))
            {
                await Task.CompletedTask;
                return;
            }

            foreach (var permissionClaim in userPermissionClaims)
            {
                var parts = permissionClaim.Value.Split('.');
                if (parts.Length != 2)
                    continue;

                var key = parts[0]; // 1
                var permission = parts[1]; // f

                if (_permissions.ContainsKey(key) && permission.Equals(PERMISSION_FULL, StringComparison.OrdinalIgnoreCase))
                {
                    await Task.CompletedTask;
                    return;
                }

                if (_permissions.TryGetValue(key, out PermissionAuth value) && value.Permission == permission)
                {
                    await Task.CompletedTask;
                    return;
                }
            }

            // Call jwtHandler middleware
            await context.HttpContext.ForbidAsync("Bearer");
            context.Result = new EmptyResult(); // chặn tiếp xử lý            
        }
    }
}