using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Core.Enums;
using System.Text.Json.Serialization;
using System.Threading;

namespace Wulkanizacja.Service.Application.Dto
{
    public class TireDto
    {
        public Guid Id { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string SpeedIndex { get; set; } = string.Empty;
        public string LoadIndex { get; set; } = string.Empty;
        public TireType TireType { get; set; }
        public string ManufactureDate { get; set; } = string.Empty;
        public DateTimeOffset? CreateDate { get; set; }
        public DateTimeOffset? EditDate { get; set; }
        public string? Comments { get; set; }
        public int QuantityInStock { get; set; }

        public bool Validate()
        {
            return
                   string.IsNullOrWhiteSpace(Brand) ||
                   string.IsNullOrWhiteSpace(Model) ||
                   string.IsNullOrWhiteSpace(Size) ||
                   string.IsNullOrWhiteSpace(SpeedIndex) ||
                   string.IsNullOrWhiteSpace(LoadIndex) ||
                   !Enum.IsDefined(typeof(TireType), TireType) ||
                   string.IsNullOrWhiteSpace(ManufactureDate) ||
                   QuantityInStock < 0;
        }
    }
}
