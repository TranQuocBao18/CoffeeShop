using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CoffeeShop.Infrastructure.Identity;
using CoffeeShop.Infrastructure.Shared.Interfaces;
using CoffeeShop.Infrastructure.Shared;
using CoffeeShop.Infrastructure.Shared.Services;
using CoffeeShop.Presentation.Shared.Permissions;
using CoffeeShop.Service.Identity.Extensions;
using CoffeeShop.Presentation.Shared.Extensions;

namespace CoffeeShop.Presentation.Identity.Extensions
{
    public static class IdentityPresentationExtension
    {
        public static void AddIdentityExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();

            services.AddIdentityAuthenticationLayer(configuration);
            services.AddAuthorizationPermission();

            services.AddInfrastructureLayer(configuration);
            services.AddSharedInfrastructure(configuration);
            services.AddServiceLayer();
            services.AddOperationBuilderServices();

            services.AddSwaggerExtension();
            services.AddApiVersioningExtension();
        }
    }
}