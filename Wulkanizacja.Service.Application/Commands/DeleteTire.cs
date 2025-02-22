using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Convey.CQRS.Commands;

namespace Wulkanizacja.Service.Application.Commands
{
    public class DeleteTire : ICommand
    {
        public DeleteTire(Guid tireId)
        {
            TireId = tireId;
        }

        public Guid TireId { get; set; }
    }
}
