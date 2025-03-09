using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wulkanizacja.Service.Api.Exceptions
{
    public abstract class ApiExceptions : Exception
    {
        #region Properties

        public abstract string Code { get; }

        #endregion Properties

        #region Constructors

        protected ApiExceptions(string message) : base(message)
        {
        }

        #endregion Constructors
    }
}
