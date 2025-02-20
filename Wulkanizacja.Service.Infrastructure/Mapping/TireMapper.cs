using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Core.Aggregates;
using Wulkanizacja.Service.Infrastructure.Postgres.Entities;

namespace Wulkanizacja.Service.Infrastructure.Mapping
{
    public static class TireMapper
    {
        public static TireRecord ToRecord(this TireAggregate mainOrder)
    => new()
    {
        TireId = mainOrder.Id,
        Brand = mainOrder.Brand,
        Size = mainOrder.Size
    };

    }
}
