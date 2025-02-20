using Microsoft.EntityFrameworkCore;
using Npgsql;
using Polly;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wulkanizacja.Service.Infrastructure.Postgres.Context
{
    public class PersistenceContext : DbContext, IPersistenceContext
    {
        #region Properties

        public IDbConnection Connection
            => new NpgsqlConnection(Database.GetConnectionString());

        #endregion

        #region Constructors

        public PersistenceContext()
        {
        }

        protected PersistenceContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        #endregion

        #region Methods

        public virtual Task SeedAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Database.AutoTransactionBehavior = AutoTransactionBehavior.Always;
            base.OnConfiguring(optionsBuilder);
        }
    }
}
