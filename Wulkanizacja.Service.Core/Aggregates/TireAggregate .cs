using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Core.Enums;
using Wulkanizacja.Service.Core.Events;
using Wulkanizacja.Service.Core.Models;

namespace Wulkanizacja.Service.Core.Aggregates
{
    public class TireAggregate : AggregateRoot
    {
        public string Brand { get; private set; }
        public string Model { get; private set; }
        public string Size { get; private set; }
        public string SpeedIndex { get; private set; }
        public string LoadIndex { get; private set; }
        public TireType Type { get; private set; }
        public DateTimeOffset? ManufactureDate { get; private set; }
        public DateTimeOffset? CreateDate { get; private set; }
        public DateTimeOffset? EditDate { get; private set; }
        public string? Comments { get; private set; }
        public int QuantityInStock { get; private set; } 

        public TireAggregate(TireModel tireModel, AggregateId id)
        {
            Id = new AggregateId();
            Brand = tireModel.Brand;
            Model = tireModel.Model;
            Size = tireModel.Size;
            SpeedIndex = tireModel.SpeedIndex;
            LoadIndex = tireModel.LoadIndex;
            Type = tireModel.TireType;
            ManufactureDate = tireModel.ManufactureDate;
            CreateDate = DateTimeOffset.UtcNow;
            EditDate = tireModel.EditDate;
            Comments = tireModel.Comments;
            QuantityInStock = tireModel.QuantityInStock;
        }

        public TireAggregate(TireModel tireModel) : this(tireModel,
            new AggregateId())
        {
        }

        public void AddTire()
        {
            AddEvent(new AddTireEvent(this));
        }
    }
}



