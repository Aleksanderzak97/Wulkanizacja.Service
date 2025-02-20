using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Core.Aggregates;
using Wulkanizacja.Service.Core.Repositories;
using Wulkanizacja.Service.Infrastructure.Mapping;
using Wulkanizacja.Service.Infrastructure.Postgres.Context;

namespace Wulkanizacja.Service.Infrastructure.Postgres.Repositories
{
    public class TiresRepository(TiresDbContext ordersDbContext) : ITiresRepository
    {
        public async Task<TireAggregate> CreateTire(TireAggregate mainOrder, CancellationToken cancellationToken)
        {
            await WaitForFreeTransaction(cancellationToken);

            await using var transaction = await ordersDbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                await ordersDbContext.Tires.AddAsync(mainOrder.ToRecord(), cancellationToken);
                await SaveContextChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return mainOrder;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        private async Task SaveContextChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                await ordersDbContext.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async Task WaitForFreeTransaction(CancellationToken cancellationToken)
        {
            while (ordersDbContext.Database.CurrentTransaction != null)
            {
                await Task.Delay(100, cancellationToken);
            }
        }
    }

}
