using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wulkanizacja.Service.Infrastructure.Postgres.Services
{
    public interface IDatabaseMigrationService
    {
        Task EnsureMigrationsAppliedAsync(CancellationToken cancellationToken = default);
    }
}
