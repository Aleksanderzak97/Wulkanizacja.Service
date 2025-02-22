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
            var oldTire = @event.OldTire;

            await repository.UpdateTire(@event.NewTire, oldTire, CancellationToken.None);
        }
    }
}
