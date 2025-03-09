using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Wulkanizacja.Service.Infrastructure.Postgres.Context;
using Wulkanizacja.Service.Infrastructure.Postgres.Entities;
using Xunit;

namespace Wulkanizacja.Service.Tests
{
    public class TireServiceTests
    {
        private readonly IServiceProvider _serviceProvider;

        public TireServiceTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<TiresDbContext>(options =>
                options.UseInMemoryDatabase("TestDatabase"));

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task AddTire_ShouldAddTireToDatabase()
        {
            // Arrange
            var context = _serviceProvider.GetRequiredService<TiresDbContext>();
            var tire = new TireRecord
            {
                TireId = Guid.NewGuid(),
                Brand = "Michelin",
                Model = "Pilot Sport 4",
                Size = "205/55 R16",
                TireTypeId = 1,
                SpeedIndex = "Y",
                LoadIndex = "91",
                ManufactureDate = "5225",
                CreationDate = DateTimeOffset.UtcNow,
                QuantityInStock = 10
            };

            // Act
            context.Tires.Add(tire);
            await context.SaveChangesAsync();

            // Assert
            var savedTire = await context.Tires.FindAsync(tire.TireId);
            Assert.NotNull(savedTire);
            Assert.Equal("Michelin", savedTire.Brand);
        }

        [Fact]
        public async Task UpdateTire_ShouldUpdateTireInDatabase()
        {
            // Arrange
            var context = _serviceProvider.GetRequiredService<TiresDbContext>();
            var tire = new TireRecord
            {
                TireId = Guid.NewGuid(),
                Brand = "Bridgestone",
                Model = "Turanza T005",
                Size = "195/65 R15",
                TireTypeId = 1,
                SpeedIndex = "H",
                LoadIndex = "88",
                ManufactureDate = "5225",
                CreationDate = DateTimeOffset.UtcNow.AddMonths(-3),
                QuantityInStock = 5
            };

            context.Tires.Add(tire);
            await context.SaveChangesAsync();

            // Act - pobieramy ju¿ œledzony obiekt i modyfikujemy jego w³aœciwoœci za pomoc¹ EF Core Entry API.
            var trackedTire = await context.Tires.FindAsync(tire.TireId);
            if (trackedTire != null)
            {
                var entry = context.Entry(trackedTire);
                entry.CurrentValues["Brand"] = "Goodyear";
                entry.CurrentValues["QuantityInStock"] = 7;
                entry.CurrentValues["EditDate"] = DateTimeOffset.UtcNow;
                await context.SaveChangesAsync();
            }

            // Assert
            var updatedTire = await context.Tires.FindAsync(tire.TireId);
            Assert.NotNull(updatedTire);
            Assert.Equal("Goodyear", updatedTire.Brand);
            Assert.Equal(7, updatedTire.QuantityInStock);
        }

        [Fact]
        public async Task DeleteTire_ShouldRemoveTireFromDatabase()
        {
            // Arrange
            var context = _serviceProvider.GetRequiredService<TiresDbContext>();
            var tire = new TireRecord
            {
                TireId = Guid.NewGuid(),
                Brand = "Pirelli",
                Model = "P Zero",
                Size = "225/45 R17",
                TireTypeId = 1,
                SpeedIndex = "V",
                LoadIndex = "92",
                ManufactureDate = "5225",
                CreationDate = DateTimeOffset.UtcNow.AddMonths(-1),
                QuantityInStock = 8
            };

            context.Tires.Add(tire);
            await context.SaveChangesAsync();

            // Act
            context.Tires.Remove(tire);
            await context.SaveChangesAsync();

            // Assert
            var deletedTire = await context.Tires.FindAsync(tire.TireId);
            Assert.Null(deletedTire);
        }

        [Fact]
        public async Task FindNonExistingTire_ShouldReturnNull()
        {
            // Arrange
            var context = _serviceProvider.GetRequiredService<TiresDbContext>();
            var nonExistingId = Guid.NewGuid();

            // Act
            var tire = await context.Tires.FindAsync(nonExistingId);

            // Assert
            Assert.Null(tire);
        }

        [Fact]
        public async Task AddMultipleTiresAndCount_ShouldReturnCorrectCount()
        {
            // Arrange
            var context = _serviceProvider.GetRequiredService<TiresDbContext>();

            // Usuwamy poprzednie dane z InMemoryDatabase
            context.Tires.RemoveRange(context.Tires);
            await context.SaveChangesAsync();

            var tires = Enumerable.Range(1, 5).Select(i => new TireRecord
            {
                TireId = Guid.NewGuid(),
                Brand = $"Brand{i}",
                Model = $"Model{i}",
                Size = $"{200 + i}/55 R16",
                TireTypeId = 1,
                SpeedIndex = "Y",
                LoadIndex = "91",
                ManufactureDate = "5225",
                CreationDate = DateTimeOffset.UtcNow,
                QuantityInStock = 10 + i
            }).ToList();

            // Act
            context.Tires.AddRange(tires);
            await context.SaveChangesAsync();

            // Assert
            var count = await context.Tires.CountAsync();
            Assert.Equal(5, count);
        }
    }
}
