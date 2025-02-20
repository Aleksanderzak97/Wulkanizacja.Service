using Convey;
using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Convey.Docs.Swagger;
using Wulkanizacja.Service.Core.Repositories;
using EntityFramework.Exceptions.PostgreSQL;
using Wulkanizacja.Service.Infrastructure.Postgres.Context;
using Wulkanizacja.Service.Infrastructure.Postgres.Options;
using Wulkanizacja.Service.Infrastructure.Postgres.Repositories;
using Wulkanizacja.Service.Infrastructure.Postgres.Services;
using Microsoft.AspNetCore.Builder;
using Wulkanizacja.Service.Infrastructure.Filters;
using Convey.WebApi.Swagger;

namespace Wulkanizacja.Service.Infrastructure
{
    public static class Extensions
    {
        private const string DistributedCacheSettingsSectionName = "DistributedCacheSettings";
        private const string InfrastructureSectionName = "infrastructure";

        public static IConveyBuilder AddPostgres(this IConveyBuilder builder)
        {
            builder.Services
                .AddOptions()
             
                .AddOptions<PostgresOptions>()
                .Configure<IConfiguration>((options, configuration) =>
                {
                    configuration.GetSection("postgres").Bind(options);
                });

            builder.Services
                .AddScoped<IContextsMigrationService, ContextsMigrationService>()
                .AddScoped<ITiresRepository, TiresRepository>()
                .AddDbContext<TiresDbContext>((service, options) =>
                {
                    var postgresOptions = service.GetRequiredService<IOptions<PostgresOptions>>().Value;
                    options
                        .UseNpgsql(postgresOptions.ConnectionString)
                        .UseExceptionProcessor();
                     
                });

            return builder;


        }

public static IConveyBuilder AddSwagger(this IConveyBuilder builder)
{
    builder.Services.Configure<SwaggerOptions>(options =>
    {
        options.Enabled = true;
        options.ReDocEnabled = false;
        options.Name = "v1";
        options.Title = "Wulkanizacja Service API";
        options.Version = "v1";
        options.RoutePrefix = "swagger";
        options.IncludeSecurity = false;
    });

    builder.Services.AddSingleton(resolver =>
        resolver.GetRequiredService<IOptions<SwaggerOptions>>().Value);

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Version = "v1",
            Title = "Wulkanizacja Service API",
            Description = "API for Wulkanizacja Service"
        });

        // Nie dodajemy własnych filtrów – domyślna konfiguracja AddSwaggerDocs() wystarczy
    });

            return builder.AddWebApiSwaggerDocs();


        }


    }
}
