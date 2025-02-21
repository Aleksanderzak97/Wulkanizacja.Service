using System;
using System.Collections.Generic;
using Wulkanizacja.Service.Core.Events;

namespace Wulkanizacja.Service.Core.Aggregates
{
    /// <summary>
    /// Bazowa klasa agregatu, zawierająca mechanizm przechowywania zdarzeń domenowych.
    /// </summary>
    public abstract class AggregateRoot
    {
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

        /// <summary>
        /// Zdarzenia domenowe, które zostały wygenerowane przez agregat.
        /// </summary>
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public AggregateId Id { get; protected set; }
        public int Version { get; protected set; }

        /// <summary>
        /// Dodaje zdarzenie domenowe do agregatu.
        /// </summary>
        /// <param name="domainEvent">Zdarzenie domenowe do dodania.</param>
        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            if (domainEvent == null)
                throw new ArgumentNullException(nameof(domainEvent));

            _domainEvents.Add(domainEvent);
            Version++; // Możesz rozważyć inny mechanizm wersjonowania
        }

        /// <summary>
        /// Czyści zgromadzone zdarzenia domenowe.
        /// </summary>
        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}
