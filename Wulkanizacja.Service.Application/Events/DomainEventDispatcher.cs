using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Core.Events;

namespace Wulkanizacja.Service.Application.Events
{
    internal sealed class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public DomainEventDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private async Task InvokeHandleAsync<T>(Type handlerType, IDomainEventHandler handler, IDomainEvent @event, CancellationToken cancellationToken) where T : class, IDomainEvent
        {
            if (handlerType.GetMethod(nameof(IDomainEventHandler<T>.HandleAsync)) is { } handleMethod)
            {
                await ((Task)handleMethod.Invoke(handler, new object[] { @event, cancellationToken }))!;
            }
        }

        public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class, IDomainEvent
        {
            var commandHandlerType = typeof(IDomainEventHandler<>).MakeGenericType(@event.GetType());
            var handlers = _serviceProvider.GetServices(commandHandlerType);
            foreach (var handler in handlers)
            {
                await InvokeHandleAsync<T>(commandHandlerType, (IDomainEventHandler)handler, @event, cancellationToken);
            }
        }
    }
}
