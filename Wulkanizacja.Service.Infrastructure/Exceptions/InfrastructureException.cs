using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wulkanizacja.Service.Infrastructure.Exceptions
{
    public abstract class InfrastructureException : Exception
    {
        #region Properties

        public abstract string Code { get; }

        #endregion Properties

        #region Constructors

        protected InfrastructureException(string message) : base(message)
        {
        }

        #endregion Constructors
    }
}
