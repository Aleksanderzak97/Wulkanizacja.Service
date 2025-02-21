using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Core.Aggregates;
using Wulkanizacja.Service.Core.Enums;
using Wulkanizacja.Service.Core.Models;
using Wulkanizacja.Service.Infrastructure.Postgres.Entities;

namespace Wulkanizacja.Service.Infrastructure.Mapping
{
    public static class TireMapper
    {
        public static TireRecord ToRecord(this TireAggregate tireAggregate)
       => new TireRecord
       {
           TireId = tireAggregate.Id,
           Brand = tireAggregate.Brand,
           Model = tireAggregate.Model,
           Size = tireAggregate.Size,
           SpeedIndex = tireAggregate.SpeedIndex,
           LoadIndex = tireAggregate.LoadIndex,
           TireTypeId = (short)tireAggregate.Type,
           ManufactureDate = tireAggregate.ManufactureDate,
           CreationDate = tireAggregate.CreateDate,
           EditDate = tireAggregate.EditDate,
           Comments = tireAggregate.Comments,
           QuantityInStock = tireAggregate.QuantityInStock
       };

        public static TireAggregate ToAggregate(this TireRecord tireRecord)
        {

            return new(new TireModel
            {
                Brand = tireRecord.Brand,
                Model = tireRecord.Model,
                Size = tireRecord.Size,
                SpeedIndex = tireRecord.SpeedIndex,
                LoadIndex = tireRecord.LoadIndex,
                TireType = (TireType)tireRecord.TireTypeId,
                ManufactureDate = tireRecord.ManufactureDate,
                EditDate = tireRecord.EditDate,
                Comments = tireRecord.Comments,
                QuantityInStock = tireRecord.QuantityInStock
            }
            );
        }

        public static ICollection<TireAggregate> ToAggregate(this IEnumerable<TireRecord> tires)
        => tires.Select(tire => tire.ToAggregate()).ToList();
    }
}
