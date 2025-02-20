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
    public class ContextsMigrationService : IContextsMigrationService
    {
        private readonly ILogger<ContextsMigrationService> _logger;
        private readonly TiresDbContext _operationsDbContext;

        public ContextsMigrationService(ILogger<ContextsMigrationService> logger, TiresDbContext operationsDbContext)
        {
            _logger = logger;
            _operationsDbContext = operationsDbContext;
        }

        public async Task MigrateContextsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if ((await _operationsDbContext.Database.GetPendingMigrationsAsync(cancellationToken)).ToArray() is
                    { Length: > 0 })
                {
                    await _operationsDbContext.Database.MigrateAsync(cancellationToken);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Contexts migration error {ErrorMessage}", e.Message);
                throw;
            }
        }
    }
    }
