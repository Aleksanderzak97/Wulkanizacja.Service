using Convey.CQRS.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Core.Events;

namespace Wulkanizacja.Service.Application.Events
{
    public interface IMessagePublisher
    {
        #region Methods

        Task PublishDomainEventsAsync(params IDomainEvent[] domainEvents);

        #endregion Methods
    }
}
