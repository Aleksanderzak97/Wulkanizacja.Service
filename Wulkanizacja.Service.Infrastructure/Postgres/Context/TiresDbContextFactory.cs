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
        private readonly PostgresOptions _options;

        public TiresDbContextFactory(IOptions<PostgresOptions> options)
        {
            _options = options.Value;
        }

        public TiresDbContextFactory()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Wulkanizacja.Service.Api"))
                .AddJsonFile("appsettings.json")
                .Build();

            _options = configuration.GetSection("postgres").Get<PostgresOptions>();
        }

        public TiresDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TiresDbContext>();

            optionsBuilder.UseNpgsql(_options.ConnectionString);

            return new TiresDbContext(optionsBuilder.Options);
        }
    }
}


