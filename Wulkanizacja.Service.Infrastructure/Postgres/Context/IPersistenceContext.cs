using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wulkanizacja.Service.Infrastructure.Postgres.Context
{
    public interface IPersistenceContext
    {
        #region Properties

        IDbConnection Connection { get; }
        DatabaseFacade Database { get; }

        #endregion

        #region Methods

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        #endregion
    }
}
