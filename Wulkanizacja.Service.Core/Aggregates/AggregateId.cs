using SequentialGuid;
using System;
using Wulkanizacja.Service.Core.Exceptions;

namespace Wulkanizacja.Service.Core.Aggregates
{
    public sealed class AggregateId : IEquatable<AggregateId>
    {
        /// <summary>
        /// Wewnętrzna wartość GUID.
        /// </summary>
        public Guid Value { get; }

        /// <summary>
        /// Tworzy nowy identyfikator agregatu z wygenerowanym sekwencyjnym GUID-em.
        /// Używany jest czas UTC dla spójności.
        /// </summary>
        public AggregateId() : this(SequentialGuidGenerator.Instance.NewGuid(DateTime.UtcNow))
        {
        }

        /// <summary>
        /// Tworzy nowy identyfikator agregatu z podaną wartością GUID.
        /// </summary>
        /// <param name="value">Wartość GUID.</param>
        /// <exception cref="InvalidAggregateIdException">Wyrzucany, jeśli podany GUID jest pusty.</exception>
        public AggregateId(Guid value)
        {
            if (value == Guid.Empty)
                throw new InvalidAggregateIdException(value);

            Value = value;
        }

        /// <summary>
        /// Niejawna konwersja z <see cref="AggregateId"/> do <see cref="Guid"/>.
        /// </summary>
        public static implicit operator Guid(AggregateId id) => id.Value;

        /// <summary>
        /// Niejawna konwersja z <see cref="Guid"/> do <see cref="AggregateId"/>.
        /// </summary>
        public static implicit operator AggregateId(Guid id) => new AggregateId(id);

        public bool Equals(AggregateId other)
        {
            if (other is null)
                return false;
            return ReferenceEquals(this, other) || Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is AggregateId other && Equals(other);
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value.ToString();

    }
}
