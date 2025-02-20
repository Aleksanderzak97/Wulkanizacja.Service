using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Core.Events;

namespace Wulkanizacja.Service.Core.Aggregates
{
    public abstract class AggregateRoot
    {
        private readonly List<IDomainEvent> _events = [];
        public IEnumerable<IDomainEvent> Events => _events;
        public AggregateId Id { get; protected set; }
        public int Version { get; protected set; }

        protected void AddEvent(IDomainEvent @event)
        {
            if (_events.Count == 0)
            {
                Version++;
            }

            _events.Add(@event);
        }

        public void ClearEvents() => _events.Clear();
    }
}
