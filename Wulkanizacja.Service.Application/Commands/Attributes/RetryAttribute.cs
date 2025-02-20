using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wulkanizacja.Service.Application.Commands.Attributes
{
    internal class RetryAttribute : HandlerAttributeBase
    {
        #region Properties

        public int MaxRetryAttempts { get; set; }
        public TimeSpan Delay { get; set; }
        public Type ExceptionType { get; set; }

        #endregion

        #region Constructors

        public RetryAttribute(int maxRetryAttempts, int delayInMiliseconds, Type exceptionType)
        {
            MaxRetryAttempts = maxRetryAttempts;
            Delay = TimeSpan.FromMilliseconds(delayInMiliseconds);
            ExceptionType = exceptionType;
        }

        #endregion
    }
}
