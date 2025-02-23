using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Core.Events;
using Wulkanizacja.Service.Core.Repositories;

namespace Wulkanizacja.Service.Application.Events.Handlers
{
    public class UpdateTireEventHandler(ITiresRepository repository) : IDomainEventHandler<UpdateTireEvent>
    {
        public async Task HandleAsync(UpdateTireEvent @event, CancellationToken cancellationToken = default)
        {
            await repository.UpdateTire(@event.tire, @event.oldTire, CancellationToken.None);
        }
    }
}
