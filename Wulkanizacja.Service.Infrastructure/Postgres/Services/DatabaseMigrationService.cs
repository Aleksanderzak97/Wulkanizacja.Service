using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wulkanizacja.Service.Infrastructure.Postgres.Context;

namespace Wulkanizacja.Service.Infrastructure.Postgres.Services
{
    public class DatabaseMigrationService : IDatabaseMigrationService
    {
        private readonly ILogger<DatabaseMigrationService> _logger;
        private readonly TiresDbContext _dbContext ;

        public DatabaseMigrationService(ILogger<DatabaseMigrationService> logger, TiresDbContext dbContext)
        {
            _logger = logger;
            _dbContext  = dbContext;
        }

        public async Task EnsureMigrationsAppliedAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync(cancellationToken);

                if (pendingMigrations.Any())
                {
                    _logger.LogInformation("Found {Count} pending migrations.", pendingMigrations.Count());
                    await _dbContext.Database.MigrateAsync(cancellationToken);
                    _logger.LogInformation("Migrations applied successfully.");
                }
                else
                {
                    _logger.LogInformation("No pending migrations found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while applying migrations: {Message}", ex.Message);
                throw;
            }
        }

    }
}
