using Microsoft.Extensions.Logging;
using Polly.Retry;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Convey.CQRS.Commands;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Application.Commands.Attributes;

namespace Wulkanizacja.Service.Application.Commands.Handlers
{
    public abstract class CommandHandlerBase<TCommand> : ICommandHandler<TCommand> where TCommand : class, ICommand
    {
        #region Fields

        protected ILogger Logger { get; }

        #endregion Fields

        #region Constructors

        protected CommandHandlerBase(ILogger logger)
        {
            Logger = logger;
        }

        #endregion Constructors

        #region Methods

        private Attribute?[] GetHandlerAttributes()
        {
            var methodInfo = GetType().GetMethod(nameof(HandleCommandAsync), BindingFlags.Public | BindingFlags.Instance);
            var attributes = methodInfo?.GetCustomAttributes(true);

            return attributes?.Where(a => a is CommandHandlerAttributeBase)
                .Select(a => a as Attribute).ToArray() ?? Array.Empty<Attribute?>();
        }

        public abstract Task HandleCommandAsync(TCommand command, CancellationToken cancellationToken = default);

        protected virtual ResiliencePipeline BuildPipeline(params Attribute?[] attributes)
        {
            var pipelineBuilder = new ResiliencePipelineBuilder();

            foreach (var attribute in attributes)
                if (attribute is AutoRetryOnExceptionAttribute retryAttribute)
                {
                    var retryOptions = new RetryStrategyOptions
                    {
                        Delay = retryAttribute.Delay,
                        MaxRetryAttempts = retryAttribute.MaxRetryAttempts
                        ,
                        ShouldHandle = new PredicateBuilder().Handle(RetryService.Handle(retryAttribute.ExceptionType))
                        ,
                        OnRetry = args =>
                        {
                            Logger.LogWarning(args.Outcome.Exception
                                , "An error {ErrorMessage} occurred on retry {RetryAttempt} for operation {OperationKey}",
                                args.Outcome.Exception?.Message, args.AttemptNumber, args.Context.OperationKey);
                            return ValueTask.CompletedTask;
                        }
                    };

                    pipelineBuilder.AddRetry(retryOptions);
                }
                else if (attribute is TimeoutAttribute timeoutAttribute)
                {
                    pipelineBuilder.AddTimeout(timeoutAttribute.Timeout);
                }

            return pipelineBuilder.Build();
        }

        #endregion Methods

        #region Interface Members

        public async Task HandleAsync(TCommand command, CancellationToken cancellationToken = default)
        {
            var handlerAttributesArray = GetHandlerAttributes();
            if (handlerAttributesArray.Length == 0) await HandleCommandAsync(command, cancellationToken);

            var pipeline = BuildPipeline(handlerAttributesArray);
            await pipeline.ExecuteAsync(async t => await HandleCommandAsync(command, cancellationToken), cancellationToken);
        }

        #endregion Interface Members
    }
}
