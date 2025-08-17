using System.Security.Claims;
using CoffeeShop.Domain.Shared.Enums;
using CoffeeShop.Infrastructure.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeShop.Presentation.Shared.Permissions
{
    public static class PermissionFixedRolesExtension
    {
        public static void AddAuthorizationPermission(this IServiceCollection services)
        {
            services.AddAuthorization(SetupAuthzPolicy);
        }

        private static void SetupAuthzPolicy(AuthorizationOptions options)
        {
            options.AddPolicy(AuthzPolicy.ALLOW_SUPPER_ADMIN_ACCESS, policy =>
            {
                policy.RequireAssertion(c => c.User.HasClaim(x => x.Type == ClaimTypes.Role && x.Value == RolesEnum.SuperAdmin.ToString()));
            });

            options.AddPolicy(AuthzPolicy.ALLOW_CUSTOMER_ACCESS, policy =>
            {
                policy.RequireAssertion(c => c.User.HasClaim(x => x.Type == ClaimTypes.Role && x.Value == RolesEnum.Customer.ToString()));
            });

            // options.AddPolicy(AuthzPolicy.ALLOW_BASIC_ACCESS, policy =>
            // {
            //     policy.RequireAssertion(c => c.User.HasClaim(x =>
            //            x.Type == IdentityConstant.Permission
            //            && (x.Value == ApplicationPermissions.Product.FULL
            //            || x.Value == ApplicationPermissions.Customer.FULL)
            //        ));
            // });
        }
    }
}