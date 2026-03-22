using System;
using Wulkanizacja.Service.Application.Events;
using Wulkanizacja.Service.Core.Aggregates;
using Wulkanizacja.Service.Application.CQRS.Commands;

namespace Wulkanizacja.Service.Application.Commands.Handlers
{
    internal class DeleteTireHandler(IMessagePublisher publisher)
           : ICommandHandler<DeleteTire>
    {
        public async Task HandleAsync(DeleteTire command, CancellationToken cancellationToken = default)
        {
            var tire = new TireAggregate(command.TireId);
            tire.DeleteTire();

            await publisher.PublishDomainEventsAsync(tire.DomainEvents.ToArray());
            tire.ClearDomainEvents();

        }
    }
}
