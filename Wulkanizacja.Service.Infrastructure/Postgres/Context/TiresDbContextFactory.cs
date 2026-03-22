using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.IO;
using Wulkanizacja.Service.Infrastructure.Postgres.Options;

namespace Wulkanizacja.Service.Infrastructure.Postgres.Context
{
    public class TiresDbContextFactory : IDesignTimeDbContextFactory<TiresDbContext>
    {
        private readonly PostgresOptions _options = new();

        public TiresDbContextFactory(IOptions<PostgresOptions> options)
        {
            _options = options.Value ?? new PostgresOptions();
        }

        public TiresDbContextFactory()
        {
            var appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "../Wulkanizacja.Service.Api/appsettings.json");

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(appSettingsPath)
                .Build();

            _options = configuration.GetSection("postgres").Get<PostgresOptions>() ?? new PostgresOptions();
        }

        public TiresDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TiresDbContext>();

            optionsBuilder.UseNpgsql(_options.ConnectionString);

            return new TiresDbContext(optionsBuilder.Options);
        }
    }
}


