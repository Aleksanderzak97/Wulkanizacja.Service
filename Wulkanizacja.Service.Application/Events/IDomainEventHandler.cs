using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Core.Events;

namespace Wulkanizacja.Service.Application.Events
{
    public interface IDomainEventHandler
    {
    }

    public interface IDomainEventHandler<in T> : IDomainEventHandler
        where T : class, IDomainEvent
    {
        Task HandleAsync(T @event, CancellationToken cancellationToken = default);
    }
}
