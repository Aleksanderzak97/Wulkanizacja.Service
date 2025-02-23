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
using Wulkanizacja.Service.Application.Converters;

namespace Wulkanizacja.Service.Application.Commands.Handlers
{
    internal class PostTireHandler(ILogger<PostTireHandler> logger, IMessagePublisher publisher, WeekYearToDateConverter weekYearToDateConverter) : CommandHandlerBase<PostTire>(logger)
    {
        private readonly WeekYearToDateConverter _weekYearToDateConverter = weekYearToDateConverter;


        [AutoRetryOnException(2, 1000, typeof(DBConcurrencyException))]

        public override async Task HandleCommandAsync(PostTire command, CancellationToken cancellationToken = default)
        {
            command.Tire.ManufactureDate = _weekYearToDateConverter.ConvertWeekYearToDate(command.Tire.ManufactureWeekYear).ToUniversalTime();
            var tireId = command.Tire.Id == Guid.Empty ? Guid.NewGuid() : command.Tire.Id;
            command.Tire.Id = tireId;
            var tire = new TireAggregate(command.Tire.ToModel());
            tire.AddTire();

            await publisher.PublishDomainEventsAsync(tire.DomainEvents.ToArray());
            await Task.CompletedTask;
        }

    }
}
