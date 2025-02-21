using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Application.Events;
using System.Data;
using Wulkanizacja.Service.Application.Commands.Attributes;
using Wulkanizacja.Service.Core.Aggregates;
using Convey.CQRS.Events;
using Wulkanizacja.Service.Core.Repositories;
using Wulkanizacja.Service.Application.Mapping;

namespace Wulkanizacja.Service.Application.Commands.Handlers
{
    internal class PostTireHandler(ILogger<PostTireHandler> logger, IMessagePublisher publisher, ITiresRepository repository) : CommandHandlerBase<PostTire>(logger)
    {
        [AutoRetryOnException(2, 1000, typeof(DBConcurrencyException))]

        public override async Task HandleCommandAsync(PostTire command, CancellationToken cancellationToken = default)
        {
            var tire = new TireAggregate(command.Tire.ToModel());
            tire.AddTire();

            await publisher.PublishDomainEventsAsync(tire.DomainEvents.ToArray());
            await Task.CompletedTask;
        }

    }
}
