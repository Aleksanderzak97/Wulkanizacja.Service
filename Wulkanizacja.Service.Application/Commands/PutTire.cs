using Convey.CQRS.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wulkanizacja.Service.Application.Commands
{
    public class PutTire : ICommand
    {
        public Guid TireId { get; set; }
        public string? Brand { get; set; } = null;
        public string? Model { get; set; } = null;
        public string? Size { get; set; } = null;
        public string? SpeedIndex { get; set; } = null;
        public string? LoadIndex { get; set; } = null;
        public short? TireType { get; set; } = null;
        public DateTimeOffset? ManufactureDate { get; set; } = null;
        public string? ManufactureWeekYear { get; set; } = null;
        public string? Comments { get; set; } = null;
        public int? QuantityInStock { get; set; } = null;

        public void SetTireId(Guid tireId)
        {
            TireId = tireId;
        }
    }
}
