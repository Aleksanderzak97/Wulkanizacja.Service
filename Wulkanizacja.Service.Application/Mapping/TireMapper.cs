using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Application.Dto;
using Wulkanizacja.Service.Core.Models;

namespace Wulkanizacja.Service.Application.Mapping
{
    public static class TireMapper
    {
        public static TireModel ToModel(this TireDto dto)
            => new()
            {
                TireType = dto.TireType,
                Id = dto.Id,
                Brand = dto.Brand,
                Model = dto.Model,
                Size = dto.Size,
                SpeedIndex = dto.SpeedIndex,
                LoadIndex = dto.LoadIndex,
                ManufactureDate = dto.ManufactureDate,
                CreateDate = dto.CreateDate,
                EditDate = dto.EditDate,
                Comments = dto.Comments,
                QuantityInStock = dto.QuantityInStock
            };

        public static TireDto ToDto(this TireModel model)
         => new()
         {
             TireType = model.TireType,
             Id = model.Id,
             Brand = model.Brand,
             Model = model.Model,
             Size = model.Size,
             SpeedIndex = model.SpeedIndex,
             LoadIndex = model.LoadIndex,
             ManufactureDate = model.ManufactureDate,
             CreateDate = model.CreateDate,
             EditDate = model.EditDate,
             Comments = model.Comments,
             QuantityInStock = model.QuantityInStock
         };
    }
}
