using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Application.Events;

namespace Wulkanizacja.Service.Application
{
    public static class Extensions
    {
        public static IConveyBuilder AddApplication(this IConveyBuilder builder)
        {
            return builder
                .AddCommandHandlers()
                .AddQueryHandlers()
                .AddInMemoryCommandDispatcher()
                .AddInMemoryEventDispatcher()
                .AddInMemoryQueryDispatcher();
        }

        public static IApplicationBuilder UseApplication(this IApplicationBuilder app)
        {
            return app;
        }

        public static IConveyBuilder AddMessaging(this IConveyBuilder builder)
        {
            builder.AddInMemoryDomainEventDispatcher();
            builder.Services.AddTransient<IMessagePublisher, MessagePublisher>();
            return builder;
        }

        internal static IConveyBuilder AddInMemoryDomainEventDispatcher(this IConveyBuilder builder)
        {
            builder.Services.Scan(s =>
            {
                s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                    .AddClasses(c => c.AssignableTo(typeof(IDomainEventHandler<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime();
            });
            builder.Services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();
            return builder;
        }


    }
}
