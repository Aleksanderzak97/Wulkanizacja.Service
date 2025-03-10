﻿using System;
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
        public string ShortSerialNumber { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Size { get; set; }
        public string SpeedIndex { get; set; }
        public string LoadIndex { get; set; }
        public string ManufactureDate { get; set; }
        public DateTimeOffset? CreateDate { get; set; }
        public DateTimeOffset? EditDate { get; set; }
        public string? Comments { get; set; }
        public int QuantityInStock { get; set; }
    }
}
