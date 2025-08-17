using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CoffeeShop.Infrastructure.Shared.Interfaces;
using CoffeeShop.Infrastructure.Identity.Interfaces;
using CoffeeShop.Infrastructure.Identity.UnitOfWorks;
using CoffeeShop.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CoffeeShop.Domain.Identity.Entities;
using CoffeeShop.Infrastructure.Identity.Contexts;
using CoffeeShop.Infrastructure.Shared.Interfaces.Repositories;
using CoffeeShop.Utilities.Extensions;
using CoffeeShop.Infrastructure.Shared.Persistences.Repositories.Common;

namespace CoffeeShop.Infrastructure.Identity
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<IdentityContext>(options =>
                    options.UseInMemoryDatabase("IdentityDb"), ServiceLifetime.Transient);
            }
            else
            {
                services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("identity"),
                    b => b.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)
                ), ServiceLifetime.Transient);
            }

            var optionLockout = configuration.GetOptions<LockoutOptions>("IdentityServiceOptions:Lockout");
            services.AddIdentity<ApplicationUser, SystemRole>(x =>
                    {
                        x.Lockout = optionLockout;
                    })
                    .AddEntityFrameworkStores<IdentityContext>()
                    .AddDefaultTokenProviders()
                    .AddSignInManager();

            services.AddScoped<IUserSettingRepository, UserSettingRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();

            services.AddScoped<IIdentityUnitOfWork, IdentityUnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepositoryAsync<,>));
        }
    }
}
