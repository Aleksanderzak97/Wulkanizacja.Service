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

        public static AsyncRetryPolicy CreatePolicy(RetryAttribute retryAttribute, ILogger logger)
        {
            var delay = retryAttribute.Delay;
            var maxRetryAttempts = retryAttribute.MaxRetryAttempts;
            var exceptionType = retryAttribute.ExceptionType;

            var retryPolicyType = typeof(Policy)
                .GetMethod(nameof(Policy.Handle), new[] { typeof(Func<Exception, bool>) })?
                .MakeGenericMethod(exceptionType);

            var policyBuilder = retryPolicyType?.Invoke(null, new object[] { Handle(exceptionType) }) as PolicyBuilder;

            return policyBuilder.WaitAndRetryAsync(
                maxRetryAttempts,
                retryAttempt => delay,
                (exception, timeSpan, retryCount, context) =>
                {
                    logger.LogError(exception, "An error occurred on retry {RetryCount} for operation {OperationKey}", retryCount, context.OperationKey);
                }
            );
        }

        #endregion Static Helpers
    }
}
