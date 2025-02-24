using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wulkanizacja.Service.Infrastructure.Postgres.Context;
using Wulkanizacja.Service.Infrastructure.Postgres.Services;

namespace Wulkanizacja.Service.Tests
{
    public class TestDatabaseMigrationService : IDatabaseMigrationService
    {
        private readonly TiresDbContext _context;

        public TestDatabaseMigrationService(TiresDbContext context)
        {
            _context = context;
        }

        public async Task EnsureMigrationsAppliedAsync(CancellationToken cancellationToken = default)
        {
            await _context.Database.EnsureCreatedAsync(cancellationToken);
        }
    }
}
