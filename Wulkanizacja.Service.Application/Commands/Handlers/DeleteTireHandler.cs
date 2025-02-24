using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Application.Events;
using Wulkanizacja.Service.Core.Aggregates;
using Wulkanizacja.Service.Core.Repositories;

namespace Wulkanizacja.Service.Application.Commands.Handlers
{
    internal class DeleteTireHandler(ILogger<DeleteTireHandler> logger, IMessagePublisher publisher)
           : ICommandHandler<DeleteTire>
    {
        public async Task HandleAsync(DeleteTire command, CancellationToken cancellationToken = default)
        {
            var tire = new TireAggregate(command.TireId);
            tire.DeleteTire();

            await publisher.PublishDomainEventsAsync(tire.DomainEvents.ToArray());
            await Task.CompletedTask;

        }
    }
}
