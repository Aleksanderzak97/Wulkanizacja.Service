﻿using System;
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
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Size { get; set; }
        public string SpeedIndex { get; set; }
        public string LoadIndex { get; set; }
        public TireType TireType { get; set; }
        public string ManufactureDate { get; set; }
        public DateTimeOffset? CreateDate { get; set; }
        public DateTimeOffset? EditDate { get; set; }
        public string? Comments { get; set; }
        public int QuantityInStock { get; set; }

        public bool Validate()
        {
            return 
                   Brand == null ||
                   Model == null ||
                   Size == null ||
                   SpeedIndex == null ||
                   LoadIndex == null ||
                   TireType == null ||
                   ManufactureDate == null ||
                   QuantityInStock == null;
        }
    }
}
