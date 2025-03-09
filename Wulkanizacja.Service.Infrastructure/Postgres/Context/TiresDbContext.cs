using Microsoft.EntityFrameworkCore;
using Wulkanizacja.Service.Core.Aggregates;
using Wulkanizacja.Service.Infrastructure.Postgres.Entities;

namespace Wulkanizacja.Service.Infrastructure.Postgres.Context
{
    public class TiresDbContext : DbContext
    {
        public DbSet<TireRecord> Tires { get; set; }
        public DbSet<TireTypeRecord> TireTypes { get; set; }

        public TiresDbContext(DbContextOptions<TiresDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TireRecord>(entity =>
            {
                entity.HasKey(e => e.TireId);
                entity.Property(e => e.Brand).IsRequired();
                entity.Property(e => e.Model).IsRequired();
                entity.Property(e => e.Size).IsRequired();
                entity.Property(e => e.TireTypeId).IsRequired();
                entity.Property(e => e.SpeedIndex).IsRequired();
                entity.Property(e => e.LoadIndex).IsRequired();
                entity.Property(e => e.ManufactureDate).IsRequired();
                entity.Property(e => e.CreationDate).HasColumnType("timestamp with time zone");
                entity.Property(e => e.EditDate).HasColumnType("timestamp with time zone");
                entity.Property(e => e.QuantityInStock).IsRequired();
            });

            modelBuilder.Entity<TireTypeRecord>(entity =>
            {
                entity.HasKey(e => e.TireTypeId);
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<TireTypeRecord>().HasData(
                new TireTypeRecord { TireTypeId = 1, Name = "Letnia" },
                new TireTypeRecord { TireTypeId = 2, Name = "Zimowa" },
                new TireTypeRecord { TireTypeId = 3, Name = "Całoroczna" }
            );
        }




    }
}
