using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Core.Aggregates;

namespace Wulkanizacja.Service.Core.Events
{
    public record UpdateTireEvent(TireAggregate tire, TireAggregate oldTire) : IDomainEvent;
}
