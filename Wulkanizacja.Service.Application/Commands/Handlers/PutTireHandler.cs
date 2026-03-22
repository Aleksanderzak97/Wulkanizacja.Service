using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Wulkanizacja.Service.Application.Commands.Attributes;
using Wulkanizacja.Service.Application.Events;
using Wulkanizacja.Service.Application.Mapping;
using Wulkanizacja.Service.Application.Services;
using Wulkanizacja.Service.Core.Aggregates;
using Wulkanizacja.Service.Core.Enums;
using Wulkanizacja.Service.Core.Repositories;
using Wulkanizacja.Service.Application.CQRS.Commands;

namespace Wulkanizacja.Service.Application.Commands.Handlers
{
    internal class PutTireHandler(ILogger<PutTireHandler> logger, IMessagePublisher publisher, TireUpdater tireUpdater)
        : CommandHandlerBase<PutTire>(logger)
    {
        [AutoRetryOnException(2, 1000, typeof(DBConcurrencyException))]

        public override async Task HandleCommandAsync(PutTire command, CancellationToken cancellationToken = default)
        {

            var updatedTire = await tireUpdater.UpdateTireAsync(command, cancellationToken);

            if (updatedTire == null)
            {
                logger.LogInformation("Brak zmian - opona o ID {TireId} nie została zaktualizowana.", command.TireId);
                return;
            }

            if (updatedTire.DomainEvents.Any())
            {
                await publisher.PublishDomainEventsAsync(updatedTire.DomainEvents.ToArray());
                await Task.CompletedTask;
            }
        }

    }
}
