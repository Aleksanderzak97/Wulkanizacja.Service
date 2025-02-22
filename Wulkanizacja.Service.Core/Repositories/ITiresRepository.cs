using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Core.Aggregates;
using Wulkanizacja.Service.Core.Enums;

namespace Wulkanizacja.Service.Core.Repositories
{
    public interface ITiresRepository
    {
        Task<TireAggregate> CreateTire(TireAggregate mainOrder, CancellationToken cancellationToken);

        Task<IEnumerable<TireAggregate>> GetBySizeAndTypeAsync(string size, TireType tiretype, CancellationToken cancellationToken);

        Task<TireAggregate> GetByIdAsync(Guid tireId, CancellationToken cancellationToken);

        Task<TireAggregate> UpdateTire(TireAggregate updatedTire, TireAggregate oldTire, CancellationToken cancellationToken);

    }
}
