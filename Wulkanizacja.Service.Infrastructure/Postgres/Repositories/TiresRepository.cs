using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Core.Aggregates;
using Wulkanizacja.Service.Core.Enums;
using Wulkanizacja.Service.Core.Repositories;
using Wulkanizacja.Service.Infrastructure.Exceptions;
using Wulkanizacja.Service.Infrastructure.Mapping;
using Wulkanizacja.Service.Infrastructure.Postgres.Context;

namespace Wulkanizacja.Service.Infrastructure.Postgres.Repositories
{
    public class TiresRepository(TiresDbContext tiresDbContext) : ITiresRepository
    {
        public async Task<TireAggregate> CreateTire(TireAggregate tireAggregate, CancellationToken cancellationToken)
        {
            await WaitForFreeTransaction(cancellationToken);

            await using var transaction = await tiresDbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                await tiresDbContext.Tires.AddAsync(tireAggregate.ToRecord(), cancellationToken);
                await SaveContextChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return tireAggregate;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }


        public async Task<IEnumerable<TireAggregate>> GetBySizeAndTypeAsync(string size, TireType tiretype, CancellationToken cancellationToken)
        {
            await WaitForFreeTransaction(cancellationToken);

            var tireRecords = await tiresDbContext.Tires
                .Where(d => d.Size == size)
                .Where(d => d.TireType.TireTypeId == (short)tiretype)
                .Select(d => d.ToAggregate())
                .ToListAsync(cancellationToken);

            if (tireRecords == null || !tireRecords.Any())
                return Enumerable.Empty<TireAggregate>();

            return tireRecords.Select(t => new TireAggregate(t.ToModel(), t.Id, t.CreateDate, t.EditDate));

        }
        public async Task<TireAggregate> GetByIdAsync(Guid tireId, CancellationToken cancellationToken)
        {
            await WaitForFreeTransaction(cancellationToken);

            var tireRecord = await tiresDbContext.Tires
            .Where(d => d.TireId == tireId)
            .Select(d => d.ToAggregate())
            .FirstOrDefaultAsync(cancellationToken);

            if (tireRecord == null) return null;

            return tireRecord;
        }


        public async Task<TireAggregate> UpdateTire(TireAggregate updatedTire, TireAggregate oldTire, CancellationToken cancellationToken)
        {
            await WaitForFreeTransaction(cancellationToken);

            await using var transaction = await tiresDbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var deviceEntity = await tiresDbContext.Tires.FirstOrDefaultAsync(d =>
                    d.TireId == oldTire.Id, cancellationToken);

                if (deviceEntity is null)
                    throw new TireNotFoundForUpdateException("Nie znaleziono w bazie opony do zaktualizowania.");

                tiresDbContext.Tires.Remove(deviceEntity);
                await tiresDbContext.Tires.AddAsync(updatedTire.ToRecord(), cancellationToken);

                await SaveContextChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return updatedTire;
            }
            catch (TireNotFoundForUpdateException ex)
            {
                Debug.WriteLine(ex.Message);
                await transaction.RollbackAsync(cancellationToken);
                throw;
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
                await tiresDbContext.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async Task WaitForFreeTransaction(CancellationToken cancellationToken)
        {
            while (tiresDbContext.Database.CurrentTransaction != null)
            {
                await Task.Delay(100, cancellationToken);
            }
        }

    }

}
