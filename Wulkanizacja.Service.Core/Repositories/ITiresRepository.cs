using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Core.Aggregates;

namespace Wulkanizacja.Service.Core.Repositories
{
    public interface ITiresRepository
    {
        Task<TireAggregate> CreateTire(TireAggregate mainOrder, CancellationToken cancellationToken);

    }
}
