using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Application.Converters;
using Wulkanizacja.Service.Core.Aggregates;
using Wulkanizacja.Service.Core.Enums;
using Wulkanizacja.Service.Core.Repositories;
using Wulkanizacja.Service.Infrastructure.Exceptions;
using Wulkanizacja.Service.Infrastructure.Mapping;
using Wulkanizacja.Service.Infrastructure.Postgres.Context;

namespace Wulkanizacja.Service.Infrastructure.Postgres.Repositories
{
    public class TiresRepository(TiresDbContext tiresDbContext, TireTypeToLocalizedStringConverter tireTypeConverter) : ITiresRepository
    {
        private readonly TireTypeToLocalizedStringConverter _tireTypeConverter = tireTypeConverter;

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
                throw new TireNotFoundException($"Nie znaleziono opon o rozmiarze {size} i typie {_tireTypeConverter.Convert(tiretype)}");

            return tireRecords.Select(t => new TireAggregate(t.ToModel(), t.Id, t.CreateDate, t.EditDate));

        }

        public async Task<TireAggregate> GetByIdAsync(Guid tireId, CancellationToken cancellationToken)
        {
            await WaitForFreeTransaction(cancellationToken);

            var tireRecord = await tiresDbContext.Tires
            .Where(d => d.TireId == tireId)
            .Select(d => d.ToAggregate())
            .FirstOrDefaultAsync(cancellationToken);

            if (tireRecord == null)
                throw new TireNotFoundException($"Nie znaleziono opony o ID {tireId}");

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

        public async Task DeleteAsync(Guid tireId, CancellationToken cancellationToken)
        {
            await WaitForFreeTransaction(cancellationToken);
            await using var transaction = await tiresDbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var tire = await tiresDbContext.Tires.FirstOrDefaultAsync(t => t.TireId == tireId, cancellationToken);
                if (tire == null)
                    throw new TireNotFoundForDeleteException("Nie znaleziono w bazie opony do usunięcia.");


                tiresDbContext.Tires.Remove(tire);
                await tiresDbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

            }
            catch (TireNotFoundForDeleteException ex)
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
