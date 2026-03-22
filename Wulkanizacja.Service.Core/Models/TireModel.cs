using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Core.Enums;

namespace Wulkanizacja.Service.Core.Models
{
    public class TireModel
    {
        public TireType TireType { get; set; }
        public Guid Id { get; set; }
        public string ShortSerialNumber { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string SpeedIndex { get; set; } = string.Empty;
        public string LoadIndex { get; set; } = string.Empty;
        public string ManufactureDate { get; set; } = string.Empty;
        public DateTimeOffset? CreateDate { get; set; }
        public DateTimeOffset? EditDate { get; set; }
        public string? Comments { get; set; }
        public int QuantityInStock { get; set; }
    }
}
