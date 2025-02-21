using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Core.Aggregates;
using Wulkanizacja.Service.Infrastructure.Postgres.Entities;

namespace Wulkanizacja.Service.Infrastructure.Mapping
{
    public static class TireMapper
    {
        public static TireRecord ToRecord(this TireAggregate tireAggregate)
       => new()
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



    }
}
