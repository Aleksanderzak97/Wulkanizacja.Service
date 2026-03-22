using Microsoft.Extensions.DependencyInjection;
using Wulkanizacja.Service.Application.CQRS.Commands;
using Wulkanizacja.Service.Application.CQRS.Queries;
using Wulkanizacja.Service.Application.Events;

namespace Wulkanizacja.Service.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(Extensions).Assembly;

        RegisterClosedImplementations(services, assembly, typeof(ICommandHandler<>));
        RegisterClosedImplementations(services, assembly, typeof(IQueryHandler<,>));

        services.AddTransient<ICommandDispatcher, CommandDispatcher>();
        services.AddTransient<IQueryDispatcher, QueryDispatcher>();

        return services;
    }

    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        RegisterClosedImplementations(services, typeof(Extensions).Assembly, typeof(IDomainEventHandler<>));
        services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddTransient<IMessagePublisher, MessagePublisher>();
        return services;
    }

    private static void RegisterClosedImplementations(IServiceCollection services, System.Reflection.Assembly assembly, Type genericInterface)
    {
        var registrations = assembly
            .GetTypes()
            .Where(type => type is { IsAbstract: false, IsInterface: false })
            .SelectMany(type => type
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericInterface)
                .Select(i => new { ServiceType = i, ImplementationType = type }));

        foreach (var registration in registrations)
        {
            services.AddTransient(registration.ServiceType, registration.ImplementationType);
        }
    }
}
