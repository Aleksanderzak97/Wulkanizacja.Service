using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wulkanizacja.Service.Core.Exceptions
{
    public abstract class DomainException : Exception
    {
        #region Properties

        public abstract string Code { get; }

        #endregion

        #region Constructors

        protected DomainException(string? message) : base(message)
        {
        }

        #endregion
    }
}
