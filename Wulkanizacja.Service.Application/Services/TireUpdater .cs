using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Application.Commands;
using Wulkanizacja.Service.Core.Aggregates;
using Wulkanizacja.Service.Core.Enums;
using Wulkanizacja.Service.Core.Repositories;

namespace Wulkanizacja.Service.Application.Services
{
    public class TireUpdater
    {
        private readonly ITiresRepository _repository;
        private readonly ILogger<TireUpdater> _logger;

        public TireUpdater(ITiresRepository repository, ILogger<TireUpdater> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<TireAggregate?> UpdateTireAsync(PutTire command, CancellationToken cancellationToken)
        {
            var tire = await _repository.GetByIdAsync(command.TireId, cancellationToken);
            if (tire == null)
            {
                _logger.LogWarning("Nie znaleziono opony o ID {TireId}", command.TireId);
                return null;
            }

            var originalTire = tire.Clone();

            ApplyChanges(tire, command);

            if (!HasChanges(tire, originalTire))
            {
                _logger.LogInformation("Brak zmian dla opony o ID {TireId}, aktualizacja zatrzymana.", command.TireId);
                return null;
            }


            tire.EditDate = DateTimeOffset.UtcNow
                .AddSeconds(-DateTimeOffset.UtcNow.Second)
                .AddMilliseconds(-DateTimeOffset.UtcNow.Millisecond);

            tire.UpdateTire(tire);

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
