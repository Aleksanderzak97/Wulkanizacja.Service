using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Core.Events;
using Wulkanizacja.Service.Core.Repositories;

namespace Wulkanizacja.Service.Application.Events.Handlers
{
    class DeleteTireEventHandler(ITiresRepository repository) : IDomainEventHandler<DeleteTireEvent>
    {
        public async Task HandleAsync(DeleteTireEvent @event, CancellationToken cancellationToken = default)
        {
            await repository.DeleteAsync(@event.TireId, cancellationToken);
        }
    }
}
