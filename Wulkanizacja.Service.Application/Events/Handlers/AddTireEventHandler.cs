using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Core.Events;
using Wulkanizacja.Service.Core.Repositories;

namespace Wulkanizacja.Service.Application.Events.Handlers
{
    public class AddTireEventHandler(ITiresRepository repository) : IDomainEventHandler<AddTireEvent>
    {
        public async Task HandleAsync(AddTireEvent @event, CancellationToken cancellationToken = default)
        {
            var createTire = await repository.CreateTire(@event.Tire, cancellationToken);
        }
    }
}
