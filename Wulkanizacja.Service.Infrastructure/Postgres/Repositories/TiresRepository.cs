using Microsoft.EntityFrameworkCore;
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
            await tiresDbContext.Tires.AddAsync(tireAggregate.ToRecord(), cancellationToken);
            await tiresDbContext.SaveChangesAsync(cancellationToken);
            return tireAggregate;
        }

        public async Task<IEnumerable<TireAggregate>> GetBySizeAndTypeAsync(string size, TireType tiretype, CancellationToken cancellationToken)
        {
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
            var tireEntity = await tiresDbContext.Tires.FirstOrDefaultAsync(d =>
                d.TireId == oldTire.Id, cancellationToken);

            if (tireEntity is null)
            {
                throw new TireNotFoundForUpdateException("Nie znaleziono w bazie opony do zaktualizowania.");
            }

            tireEntity.Brand = updatedTire.Brand;
            tireEntity.Model = updatedTire.Model;
            tireEntity.Size = updatedTire.Size;
            tireEntity.SpeedIndex = updatedTire.SpeedIndex;
            tireEntity.LoadIndex = updatedTire.LoadIndex;
            tireEntity.TireTypeId = (short)updatedTire.Type;
            tireEntity.ManufactureDate = updatedTire.ManufactureDate;
            tireEntity.CreationDate = updatedTire.CreateDate;
            tireEntity.EditDate = updatedTire.EditDate;
            tireEntity.Comments = updatedTire.Comments;
            tireEntity.QuantityInStock = updatedTire.QuantityInStock;

            await tiresDbContext.SaveChangesAsync(cancellationToken);

            return updatedTire;
        }

        public async Task DeleteAsync(Guid tireId, CancellationToken cancellationToken)
        {
            var tire = await tiresDbContext.Tires.FirstOrDefaultAsync(t => t.TireId == tireId, cancellationToken);
            if (tire == null)
            {
                throw new TireNotFoundForDeleteException("Nie znaleziono w bazie opony do usunięcia.");
            }

            tiresDbContext.Tires.Remove(tire);
            await tiresDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
