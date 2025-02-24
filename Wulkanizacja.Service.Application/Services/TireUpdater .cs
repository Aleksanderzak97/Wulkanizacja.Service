using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Application.Commands;
using Wulkanizacja.Service.Application.Converters;
using Wulkanizacja.Service.Core.Aggregates;
using Wulkanizacja.Service.Core.Enums;
using Wulkanizacja.Service.Core.Events;
using Wulkanizacja.Service.Core.Repositories;

namespace Wulkanizacja.Service.Application.Services
{
    public class TireUpdater
    {
        private readonly ITiresRepository _repository;
        private readonly ILogger<TireUpdater> _logger;
        private readonly WeekYearToDateConverter _weekYearToDateConverter;


        public TireUpdater(ITiresRepository repository, ILogger<TireUpdater> logger, WeekYearToDateConverter weekYearToDateConverter)
        {
            _repository = repository;
            _logger = logger;
            _weekYearToDateConverter = weekYearToDateConverter;
        }

        public async Task<TireAggregate?> UpdateTireAsync(PutTire command, CancellationToken cancellationToken)
        {
            if(command.ManufactureWeekYear != null)
            command.ManufactureDate = _weekYearToDateConverter.ConvertWeekYearToDate(command.ManufactureWeekYear).ToUniversalTime();
            var tire = await _repository.GetByIdAsync(command.TireId, cancellationToken);
            if (tire == null)
            {
                _logger.LogWarning("Nie znaleziono opony o ID {TireId}", command.TireId);
                return null;
            }

            var originalTire = new TireAggregate(
                tire.Id.Value, tire.Brand, tire.Model, tire.Size,
                tire.SpeedIndex, tire.LoadIndex, (short)tire.Type,
                tire.ManufactureDate, tire.Comments, tire.QuantityInStock
            );

            originalTire.CreateDate = tire.CreateDate.Value.ToUniversalTime();
            originalTire.EditDate = tire.EditDate.Value.ToUniversalTime();

            ApplyChanges(tire, command);

            if (!HasChanges(tire, originalTire))
            {
                _logger.LogInformation("Brak zmian dla opony o ID {TireId}, aktualizacja zatrzymana.", command.TireId);
                return null;
            }

            tire.CreateDate = originalTire.CreateDate;

            tire.EditDate = DateTimeOffset.UtcNow
                .AddSeconds(-DateTimeOffset.UtcNow.Second)
                .AddMilliseconds(-DateTimeOffset.UtcNow.Millisecond);

            tire.UpdateTire(originalTire);

            _logger.LogInformation("Zaktualizowano oponę o ID {TireId}", command.TireId);
            return tire;
        }

        private void ApplyChanges(TireAggregate tire, PutTire command)
        {
            if (!string.IsNullOrEmpty(command.Brand)) tire.Brand = command.Brand;
            if (!string.IsNullOrEmpty(command.Model)) tire.Model = command.Model;
            if (!string.IsNullOrEmpty(command.Size)) tire.Size = command.Size;
            if (!string.IsNullOrEmpty(command.SpeedIndex)) tire.SpeedIndex = command.SpeedIndex;
            if (!string.IsNullOrEmpty(command.LoadIndex)) tire.LoadIndex = command.LoadIndex;
            if (command.TireType.HasValue) tire.Type = (TireType)command.TireType.Value;
            if (command.ManufactureDate.HasValue) tire.ManufactureDate = command.ManufactureDate.Value;
            if (!string.IsNullOrEmpty(command.Comments)) tire.Comments = command.Comments;
            if (command.QuantityInStock.HasValue) tire.QuantityInStock = command.QuantityInStock.Value;
        }

        private bool HasChanges(TireAggregate tire, TireAggregate originalTire)
        {
            return tire.Brand != originalTire.Brand ||
                   tire.Model != originalTire.Model ||
                   tire.Size != originalTire.Size ||
                   tire.SpeedIndex != originalTire.SpeedIndex ||
                   tire.LoadIndex != originalTire.LoadIndex ||
                   tire.Type != originalTire.Type ||
                   tire.ManufactureDate != originalTire.ManufactureDate ||
                   tire.Comments != originalTire.Comments ||
                   tire.QuantityInStock != originalTire.QuantityInStock;
        }

    }

}
