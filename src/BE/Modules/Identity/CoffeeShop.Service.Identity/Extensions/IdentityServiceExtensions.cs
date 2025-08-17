using System.Reflection;
using System.Text;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using CoffeeShop.Domain.Shared.Settings;
using CoffeeShop.Infrastructure.Shared.Behaviours;
using CoffeeShop.Infrastructure.Shared.Interfaces;
using CoffeeShop.Shared.Auth;
using CoffeeShop.Service.Identity.Interfaces;
using CoffeeShop.Service.Identity.Services;

namespace CoffeeShop.Service.Identity.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static void AddIdentityAuthenticationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                var keyJwt = configuration["JWTSettings:Key"];
                o.RequireHttpsMetadata = true;
                o.SaveToken = true;
                o.IncludeErrorDetails = true;

                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = false,
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    ValidIssuer = configuration["JWTSettings:Issuer"],
                    ValidAudience = configuration["JWTSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(keyJwt)),
                    RequireSignedTokens = false,

                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    SaveSigninToken = true,
                    ClockSkew = TimeSpan.Zero
                };
                o.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = context =>
                    {
                        context.NoResult();
                        var result = JsonConvert.SerializeObject(new Response<string>(context.Response.StatusCode.ToString(), "Error Server"));
                        return context.Response.WriteAsync(result);
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        var result = JsonConvert.SerializeObject(new Response<string>(context.Response.StatusCode.ToString(), "You are not Authorized"));
                        return context.Response.WriteAsync(result);
                    },
                    OnForbidden = context =>
                    {
                        var result = JsonConvert.SerializeObject(new Response<string>(context.Response.StatusCode.ToString(), "You are not authorized to access this resource"));
                        return context.Response.WriteAsync(result);
                    },
                };
            });
        }

        public static void AddServiceLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddScoped<ISecurityContextAccessor, SecurityContextAccessor>();
            services.AddHttpContextAccessor();
            //services.AddAuthorization();

            services.AddTransient<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<INotificationPreparationService, NotificationPreparationService>();
        }
    }
}
