using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;

namespace Wulkanizacja.Service.Application.Commands.Attributes
{
    internal class RetryService
    {
        #region Static Helpers

        public static Func<Exception, bool> Handle(Type exceptionType)
        {
            return ex => exceptionType.IsInstanceOfType(ex);
        }

        #endregion Static Helpers
    }
}
