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
        public string? ManufactureDate { get; set; } = null;
        public string? Comments { get; set; } = null;
        public int? QuantityInStock { get; set; } = null;

        public void SetTireId(Guid tireId)
        {
            TireId = tireId;
        }
        public bool IsEmpty()
        {
            return TireId == Guid.Empty &&
                   Brand == null &&
                   Model == null &&
                   Size == null &&
                   SpeedIndex == null &&
                   LoadIndex == null &&
                   TireType == null &&
                   ManufactureDate == null &&
                   Comments == null &&
                   QuantityInStock == null;
        }
    }
}
