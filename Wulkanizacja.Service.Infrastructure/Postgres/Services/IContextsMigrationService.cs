using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wulkanizacja.Service.Infrastructure.Postgres.Services
{
    public interface IContextsMigrationService
    {
        Task MigrateContextsAsync(CancellationToken cancellationToken = default);
    }
}
