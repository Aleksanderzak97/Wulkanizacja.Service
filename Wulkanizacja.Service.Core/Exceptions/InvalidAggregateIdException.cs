using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wulkanizacja.Service.Core.Exceptions
{
    public class InvalidAggregateIdException : DomainException
    {
        #region Properties

        public override string Code => "invalid_aggregate_id";
        public Guid Id { get; }

        #endregion

        #region Constructors

        public InvalidAggregateIdException(Guid id) : base($"Invalid aggregate id: {id}")
        {
            Id = id;
        }

        #endregion
    }
}
