using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Core.Enums;
using Wulkanizacja.Service.Core.Events;

namespace Wulkanizacja.Service.Core.Aggregates
{
    public class TireAggregate : AggregateRoot
    {
        public string Brand { get; private set; }
        public string Size { get; private set; }
        public TireType Type { get; private set; }

        public TireAggregate(string brand, string size, TireType type)
        {
            Id = new AggregateId();
            Brand = brand;
            Size = size;
            Type = type;

            AddEvent(new AddTireEvent(Id, Brand, Size, Type));
        }
    }
}
