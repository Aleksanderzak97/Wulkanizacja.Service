using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Core.Events;

namespace Wulkanizacja.Service.Application.Events
{
    public interface IDomainEventDispatcher
    {
        Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class, IDomainEvent;
    }
}
