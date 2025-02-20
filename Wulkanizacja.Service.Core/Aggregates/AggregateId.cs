using SequentialGuid;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Core.Exceptions;

namespace Wulkanizacja.Service.Core.Aggregates
{
    public class AggregateId : IEquatable<AggregateId>, IEqualityComparer<AggregateId>
    {
        #region Properties

        public Guid Value { get; }

        #endregion

        #region Constructors

        public AggregateId() : this(SequentialGuidGenerator.Instance.NewGuid(DateTime.Now))
        {
        }

        public AggregateId(Guid value)
        {
            if (value == Guid.Empty)
                throw new InvalidAggregateIdException(value);

            Value = value;
        }

        #endregion

        #region Methods

        public static implicit operator Guid(AggregateId id)
            => id.Value;

        public static implicit operator AggregateId(Guid id)
        {
            return new AggregateId(id);
        }

        public bool Equals(AggregateId other)
        {
            if (ReferenceEquals(null, other))
                return false;

            return ReferenceEquals(this, other) || Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;

            return obj.GetType() == GetType() && Equals((AggregateId)obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public bool Equals([AllowNull] AggregateId x, [AllowNull] AggregateId y)
        {
            if (ReferenceEquals(null, y))
                return false;
            if (ReferenceEquals(null, x))
                return false;

            return ReferenceEquals(x, y) || Value.Equals(y.Value);
        }

        public int GetHashCode(AggregateId obj)
        {
            return obj?.Value.GetHashCode() ?? 0;
        }

        #endregion
    }
}
