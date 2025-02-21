using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Core.Aggregates;
using Wulkanizacja.Service.Core.Enums;

namespace Wulkanizacja.Service.Core.Events
{
    public record AddTireEvent(TireAggregate Tire) : IDomainEvent;

}
