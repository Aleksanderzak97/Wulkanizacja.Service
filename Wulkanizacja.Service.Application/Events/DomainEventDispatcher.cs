using Microsoft.Extensions.DependencyInjection;
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

        public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class, IDomainEvent
        {
            var handlers = _serviceProvider.GetServices<IDomainEventHandler<T>>();

            foreach (var handler in handlers)
            {
                await handler.HandleAsync(@event, cancellationToken);
            }
        }
    }
}
