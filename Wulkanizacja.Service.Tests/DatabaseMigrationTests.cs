using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Wulkanizacja.Service.Infrastructure.Postgres.Context;
using Wulkanizacja.Service.Infrastructure.Postgres.Services;
using Xunit;

namespace Wulkanizacja.Service.Tests
{
    public class DatabaseMigrationTests
    {
        private readonly IServiceProvider _serviceProvider;

        public DatabaseMigrationTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<TiresDbContext>(options =>
                options.UseInMemoryDatabase("MigrationTestDatabase"));

            services.AddTransient<IDatabaseMigrationService, TestDatabaseMigrationService>();

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task EnsureMigrationsApplied_ShouldCreateDatabase()
        {
            // Arrange
            var migrationService = _serviceProvider.GetRequiredService<IDatabaseMigrationService>();

            // Act
            await migrationService.EnsureMigrationsAppliedAsync(CancellationToken.None);

            // Assert – sprawdzamy, czy możemy nawiązać połączenie z bazą
            var context = _serviceProvider.GetRequiredService<TiresDbContext>();
            var canConnect = await context.Database.CanConnectAsync();
            Assert.True(canConnect);
        }

        [Fact]
        public async Task Database_ShouldBeEmpty_AfterMigration()
        {
            // Arrange
            var migrationService = _serviceProvider.GetRequiredService<IDatabaseMigrationService>();
            await migrationService.EnsureMigrationsAppliedAsync(CancellationToken.None);

            // Act
            var context = _serviceProvider.GetRequiredService<TiresDbContext>();
            var count = await context.Tires.CountAsync();

            // Assert – świeżo utworzona baza InMemory powinna być pusta
            Assert.Equal(0, count);
        }
    }
}
