﻿using System;
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
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Size { get; set; }
        public string SpeedIndex { get; set; }
        public string LoadIndex { get; set; }
        public TireType Type { get; set; }
        public string ManufactureDate { get; set; }
        public DateTimeOffset? CreateDate { get; set; }
        public DateTimeOffset? EditDate { get; set; }
        public string? Comments { get; set; }
        public int QuantityInStock { get; set; }


        public TireAggregate(Guid tireId)
        {
            Id = new AggregateId(tireId);
        }

        public TireAggregate(TireModel tireModel, AggregateId id)
        {
            Id = id;
            Brand = tireModel.Brand;
            Model = tireModel.Model;
            Size = tireModel.Size;
            SpeedIndex = tireModel.SpeedIndex;
            LoadIndex = tireModel.LoadIndex;
            Type = tireModel.TireType;
            ManufactureDate = tireModel.ManufactureDate;
            CreateDate = DateTimeOffset.UtcNow;
            EditDate = DateTimeOffset.UtcNow;
            Comments = tireModel.Comments;
            QuantityInStock = tireModel.QuantityInStock;
        }

        public TireAggregate(TireModel tireModel, AggregateId id, DateTimeOffset? createDate, DateTimeOffset? editDate)
        {
            Id = id;
            Brand = tireModel.Brand;
            Model = tireModel.Model;
            Size = tireModel.Size;
            SpeedIndex = tireModel.SpeedIndex;
            LoadIndex = tireModel.LoadIndex;
            Type = tireModel.TireType;
            ManufactureDate = tireModel.ManufactureDate;
            CreateDate = createDate.Value.ToLocalTime();
            EditDate = editDate.Value.ToLocalTime();
            Comments = tireModel.Comments;
            QuantityInStock = tireModel.QuantityInStock;
        }

        public TireAggregate(Guid tireId, string? brand, string? model, string? size, string? speedIndex, string? loadIndex, short? tireType,
                     string? manufactureDate, string? comments, int quantityInStock)
        {
            Id = new AggregateId(tireId);
            Brand = brand;
            Model = model;
            Size = size;
            SpeedIndex = speedIndex;
            LoadIndex = loadIndex;
            Type = (TireType)tireType;
            ManufactureDate = manufactureDate;
            EditDate = DateTimeOffset.UtcNow;
            Comments = comments;
            QuantityInStock = quantityInStock;

        }


        public TireAggregate(TireModel tireModel, DateTimeOffset? createDate, DateTimeOffset? editDate) : this(tireModel, tireModel.Id, createDate, editDate)
        {
        }
        public TireAggregate(TireModel tireModel) : this(tireModel, tireModel.Id)
        {
        }

        public void AddTire()
        {
            AddDomainEvent(new AddTireEvent(this));
        }

        public void DeleteTire()
             => AddDomainEvent(new DeleteTireEvent(Id));

        public TireAggregate UpdateTire(TireAggregate updatedTireAggregate)
        {
            AddDomainEvent(new UpdateTireEvent(this,updatedTireAggregate));
            return updatedTireAggregate;
        }

        public TireAggregate Clone()
        {
            return new TireAggregate(new TireModel
            {
                Id = this.Id,
                Brand = this.Brand,
                Model = this.Model,
                Size = this.Size,
                SpeedIndex = this.SpeedIndex,
                LoadIndex = this.LoadIndex,
                TireType = this.Type,
                ManufactureDate = this.ManufactureDate,
                EditDate = this.EditDate,
                Comments = this.Comments,
                QuantityInStock = this.QuantityInStock
            },
            this.CreateDate,
            this.EditDate);
        }
    }
}



