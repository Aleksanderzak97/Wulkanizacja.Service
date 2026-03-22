using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wulkanizacja.Service.Application.Converters;
using Wulkanizacja.Service.Application.Services;
using Wulkanizacja.Service.Core.Repositories;
using Wulkanizacja.Service.Infrastructure.Postgres.Context;
using Wulkanizacja.Service.Infrastructure.Postgres.Repositories;
using Wulkanizacja.Service.Infrastructure.Postgres.Services;

namespace Wulkanizacja.Service.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetSection("postgres:ConnectionString").Value;

        services
            .AddScoped<TireTypeToLocalizedStringConverter>()
            .AddScoped<IDatabaseMigrationService, DatabaseMigrationService>()
            .AddScoped<ITiresRepository, TiresRepository>()
            .AddScoped<TireUpdater>()
            .AddDbContext<TiresDbContext>(options =>
            {
                options
                    .UseNpgsql(connectionString)
                    .UseExceptionProcessor();
            });

        return services;
    }
}
