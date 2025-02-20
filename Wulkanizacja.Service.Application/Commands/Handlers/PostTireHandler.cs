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

namespace Wulkanizacja.Service.Application.Commands.Handlers
{
    internal class PostTireHandler(ILogger<PostTireHandler> logger, IMessagePublisher publisher, ITiresRepository repository) : CommandHandlerBase<PostTire>(logger)
    {
        [Retry(2, 1000, typeof(DBConcurrencyException))]

        public override async Task HandleCommandAsync(PostTire command, CancellationToken cancellationToken = default)
        {
            //todo obsluga
            //var createTire = await repository.CreateTire(command, cancellationToken);

            var tire = new TireAggregate(command.Brand, command.Size, command.Type);

            await publisher.PublishDomainEventsAsync(tire.Events.ToArray());
            await Task.CompletedTask;
        }

    }
}
