using System.Reflection;
using CoffeeShop.Model.Dto.Shared.Filters;
using CoffeeShop.Presentation.Shared.Builders;
using CoffeeShop.Presentation.Shared.Interfaces;
using CoffeeShop.Presentation.Shared.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace CoffeeShop.Presentation.Shared.Extensions
{
    public static class ServiceExtension
    {
        public static void AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                // c.IncludeXmlComments(string.Format(@"{0}CoffeeShop.WebApi.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "CoffeeShop API",
                    Description = "This Api will be responsible for overall data distribution and authorization.",
                    Contact = new OpenApiContact
                    {
                        Name = "CoffeeShop",
                        Email = "hello@CoffeeShop.com",
                        Url = new Uri("https://CoffeeShop.com"),
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });
            });
        }

        public static void AddApiVersioningExtension(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                // Specify the default API Version as 1.0
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number 
                config.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                config.ReportApiVersions = true;
            });
        }

        public static void AddOpenTelemetryExtension(this IServiceCollection services, IWebHostEnvironment environment, ConfigurationManager configuration, ILoggingBuilder logging)
        {
            services.Configure<OtlpExporterOptions>(o => o.Headers = $"x-otlp-api-key=SecretKey@1234");
            var resourceBuilder = ResourceBuilder.CreateDefault().AddService(
                Assembly.GetEntryAssembly()?.GetName().Name ?? "CoffeeShop.Presentation.Shared",
                serviceVersion: Assembly.GetEntryAssembly()?.GetName().Version?.ToString());
            logging.AddOpenTelemetry(logging =>
            {
                logging.IncludeFormattedMessage = true;
                logging.IncludeScopes = true;
            });
            services
                .AddMetrics()
                .AddOpenTelemetry()
                .ConfigureResource(c => c.AddService("CoffeeShop.Presentation.Shared"))
                .WithMetrics(provider =>
                {
                    provider.SetResourceBuilder(resourceBuilder)
                            .AddAspNetCoreInstrumentation()
                            .AddHttpClientInstrumentation()
                            .AddRuntimeInstrumentation();
                    provider.AddMeter("Microsoft.AspNetCore.Hosting", "Microsoft.AspNetCore.Server.Kestrel", "System.Net.Http", "CoffeeShop.Presentation.Shared");
                })
                .WithTracing(options =>
                {
                    if (environment.IsDevelopment())
                    {
                        options.SetSampler<AlwaysOnSampler>();
                    }
                    options.AddAspNetCoreInstrumentation()
                           .AddHttpClientInstrumentation();
                });
            if (!string.IsNullOrWhiteSpace(configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]))
            {
                services.Configure<OpenTelemetryLoggerOptions>(options => options.AddOtlpExporter())
                    .ConfigureOpenTelemetryMeterProvider(metrics => metrics.AddOtlpExporter())
                    .ConfigureOpenTelemetryTracerProvider(tracing => tracing.AddOtlpExporter());
            }
        }

        public static void AddOperationBuilderServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IFilterCompiler, SqlFilterCompiler>();
            serviceCollection.AddSingleton<IDictionary<string, IOperationBuilder>>(service =>
            {
                return new Dictionary<string, IOperationBuilder>()
                {
                    { "eq", service.GetRequiredService<EqualsOperationBuilder>() },
                    { "neq", service.GetRequiredService<NotEqualsOperationBuilder>() },
                    { "in", service.GetRequiredService<InOperationBuilder>() },
                    { "nin", service.GetRequiredService<NotInOperationBuilder>() },
                    { "lt", service.GetRequiredService<LessThanOperationBuilder>() },
                    { "lte", service.GetRequiredService<LessThanOrEqualsOperationBuilder>() },
                    { "gt", service.GetRequiredService<GreaterThanOperationBuilder>() },
                    { "gte", service.GetRequiredService<GreaterThanOrEqualsOperationBuilder>() },
                    { "contains", service.GetRequiredService<ContainsOperationBuilder>() },
                    { "ncontains", service.GetRequiredService<NotContainsOperationBuilder>() },
                    { "ago", service.GetRequiredService<AgoOperationBuilder>() },
                    { "between", service.GetRequiredService<BetweenOperationBuilder>() },
                    { "nbetween", service.GetRequiredService<NotBetweenOperationBuilder>() },
                    { "sw", service.GetRequiredService<StartsWithOperationBuilder>() },
                    { "nsw", service.GetRequiredService<NotStartsWithOperationBuilder>() },
                    { "ew", service.GetRequiredService<EndsWithOperationBuilder>() },
                    { "new", service.GetRequiredService<NotEndsWithOperationBuilder>() },
                };
            });
            serviceCollection.AddSingleton<EqualsOperationBuilder>();
            serviceCollection.AddSingleton<NotEqualsOperationBuilder>();
            serviceCollection.AddSingleton<InOperationBuilder>();
            serviceCollection.AddSingleton<NotInOperationBuilder>();
            serviceCollection.AddSingleton<LessThanOperationBuilder>();
            serviceCollection.AddSingleton<LessThanOrEqualsOperationBuilder>();
            serviceCollection.AddSingleton<GreaterThanOperationBuilder>();
            serviceCollection.AddSingleton<GreaterThanOrEqualsOperationBuilder>();
            serviceCollection.AddSingleton<ContainsOperationBuilder>();
            serviceCollection.AddSingleton<NotContainsOperationBuilder>();
            serviceCollection.AddSingleton<AgoOperationBuilder>();
            serviceCollection.AddSingleton<BetweenOperationBuilder>();
            serviceCollection.AddSingleton<NotBetweenOperationBuilder>();
            serviceCollection.AddSingleton<StartsWithOperationBuilder>();
            serviceCollection.AddSingleton<NotStartsWithOperationBuilder>();
            serviceCollection.AddSingleton<EndsWithOperationBuilder>();
            serviceCollection.AddSingleton<NotEndsWithOperationBuilder>();
            serviceCollection.AddSingleton<IValueParser<string>, StringParser>();
            serviceCollection.AddSingleton<IValueParser<double>, NumbericParser>();
            serviceCollection.AddSingleton<IValueParser<bool>, BoolParser>();
            serviceCollection.AddSingleton<IValueParser<Guid>, GuidParser>();
            serviceCollection.AddSingleton<IValueArrayParser<Guid>, GuidArrayParser>();
            serviceCollection.AddSingleton<IValueParser<DateTime>, DateTimeParser>();
            serviceCollection.AddSingleton<IValueArrayParser<string>, StringArrayParser>();
            serviceCollection.AddSingleton<IValueArrayParser<double>, NumbericArrayParser>();
            serviceCollection.AddSingleton<IValueArrayParser<DateTime>, DateTimeArrayParser>();
        }
    }
}