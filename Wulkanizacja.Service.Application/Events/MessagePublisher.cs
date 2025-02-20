using Convey.CQRS.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Core.Events;

namespace Wulkanizacja.Service.Application.Events
{
    public class MessagePublisher : IMessagePublisher
    {
        #region Fields

        private readonly IDomainEventDispatcher _domainEventDispatcher;

        #endregion Fields

        #region Constructors

        public MessagePublisher(
            IDomainEventDispatcher domainEventDispatcher
            , ILogger<MessagePublisher> logger)
        {
            _domainEventDispatcher = domainEventDispatcher;
        }

        #endregion Constructors

        #region Methods

        public async Task PublishDomainEventsAsync(params IDomainEvent[] domainEvents)
        {
            foreach (var @event in domainEvents)
            {
                await _domainEventDispatcher.PublishAsync(@event);
            }
        }

        #endregion Methods
    }
}
