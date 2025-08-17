using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CoffeeShop.Utilities.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authorization;
using CoffeeShop.Shared.Types;
using CoffeeShop.Shared.Auth.Constants;


namespace CoffeeShop.Shared.Auth
{
    public static class AuthExtensions
    {
        public static string GetClaim(this ClaimsPrincipal user, string type) => user.Claims.FirstOrDefault(f => f.Type == type)?.Value;

        public static IServiceCollection AddTokenManager(this IServiceCollection services, IConfiguration config)
        {
            var authOptions = config.GetOptionsExt<AuthOptions>("Auth");
            if (!authOptions.TokenManagerDisabled)
            {
                services.AddTransient<ITokenManager, TokenManager>();
            }
            else
            {
                // disable OWASP check which might effect to the real-performance of the whole system 
                services.AddTransient<ITokenManager, NoOpTokenManager>();
            }

            return services;
        }

        public static IApplicationBuilder UseTokenManager(this IApplicationBuilder app, IConfiguration config, SecurityHeadersBuilder securityHeadersBuilder = null)
        {
            var authOptions = config.GetOptions<AuthOptions>("Auth");
            if (!authOptions.TokenManagerDisabled)
            {
                var builder = securityHeadersBuilder ??
                              new SecurityHeadersBuilder().AddDefaultSecurePolicy();

                var policy = builder.Build();

                return app.UseMiddleware<TokenManagerMiddleware>(policy);
            }

            return app;
        }

        public static bool HasPermissions(this AuthorizationHandlerContext context, params string[] permissions)
        {
            if (permissions != null && permissions.Any())
            {
                var listPermissions = GetPermissionMatrix(permissions);
                return context.User.HasClaim(x => x.Type == Permissions.PERMISSION_CLAIM_TYPE && listPermissions.Distinct().Contains(x.Value));
            }
            return false;
        }

        public static void AddPolicyRequireAssertion(this AuthorizationOptions authorOption, string policyName, params string[] permissions) => authorOption.AddPolicy(policyName, policy => policy.RequireAssertion(c => c.HasPermissions(permissions)));

        public static IEnumerable<string> GetPermissionMatrix(params string[] permissions)
        {
            var listPermissions = new List<string>();
            if (permissions != null && permissions.Length != 0)
            {
                permissions.ToList().ForEach(permission =>
                {
                    if (permission[^1] == 'f')
                    {
                        // 2.f => 2.f, 2.v
                        foreach (var permissionLevel in PermissionLevelExtensions.GetCodes())
                        {
                            listPermissions.Add(permission[..^2] + permissionLevel);
                        }
                    }
                    else
                    {
                        listPermissions.Add(permission);
                    }
                });
                return listPermissions.Distinct();
            }
            return listPermissions.ToList();
        }
    }
}