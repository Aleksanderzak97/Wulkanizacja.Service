using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Infrastructure.Postgres.Entities;

namespace Wulkanizacja.Service.Infrastructure.Postgres.Context
{
    public class TiresDbContext : PersistenceContext
    {
        public DbSet<TireRecord> Tires { get; init; }
        public DbSet<TireTypeRecord> TireTypes { get; init; }

        public TiresDbContext(ILogger<TiresDbContext> logger, DbContextOptions<TiresDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public TiresDbContext(DbContextOptions<TiresDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public TiresDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseNpgsql("Database=wulkanizacja_db;Username=postgres;Password=admin;Port=5432;Host=localhost;")
                    .EnableSensitiveDataLogging();
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfiguracja encji TireTypeRecord
            modelBuilder.Entity<TireTypeRecord>(entity =>
            {
                entity.ToTable("tire_types");
                entity.HasKey(e => e.TireTypeId);
                entity.Property(e => e.TireTypeId).HasColumnName("tire_type_id").HasColumnType("smallint").ValueGeneratedNever();
                entity.Property(e => e.Name).HasColumnName("name").HasColumnType("varchar(50)").IsRequired();

                entity.HasData(
                    new TireTypeRecord { TireTypeId = 1, Name = "Summer" },
                    new TireTypeRecord { TireTypeId = 2, Name = "Winter" },
                    new TireTypeRecord { TireTypeId = 3, Name = "All-Season" }
                );
            });

            // Konfiguracja encji TireRecord
            modelBuilder.Entity<TireRecord>(entity =>
            {
                entity.ToTable("tires");
                entity.HasKey(e => e.TireId);
                entity.Property(e => e.TireId).HasColumnName("tire_id").HasColumnType("uuid").ValueGeneratedOnAdd();
                entity.Property(e => e.Brand).HasColumnName("brand").HasColumnType("varchar(100)").IsRequired();
                entity.Property(e => e.Model).HasColumnName("model").HasColumnType("varchar(100)").IsRequired();
                entity.Property(e => e.Size).HasColumnName("size").HasColumnType("varchar(20)").IsRequired();
                entity.Property(e => e.TireTypeId).HasColumnName("tire_type_id").HasColumnType("smallint").IsRequired();
                entity.Property(e => e.ManufactureDate).HasColumnName("manufacture_date").HasColumnType("timestamp with time zone");
                entity.Property(e => e.Comments).HasColumnName("comments").HasColumnType("varchar(200)");
                entity.Property(e => e.CreationDate).HasColumnName("creation_date").HasColumnType("timestamp with time zone").ValueGeneratedOnAdd();

                // Relacja do TireTypeRecord
                entity.HasOne(e => e.TireType)
                    .WithMany()
                    .HasForeignKey(e => e.TireTypeId)
                    .HasConstraintName("FK_Tires_TireTypes")
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
