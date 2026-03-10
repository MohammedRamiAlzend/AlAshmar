using AlAshmar.Application.Common;
using Microsoft.Extensions.DependencyInjection;

namespace AlAshmar.Infrastructure;

/// <summary>
/// Extension methods for registering command and query handlers.
/// </summary>
public static class HandlerRegistration
{
    /// <summary>
    /// Registers all command and query handlers in the application.
    /// </summary>
    public static IServiceCollection RegisterHandlers(this IServiceCollection services)
    {
        // Register all IQueryHandler implementations
        var queryHandlerTypes = typeof(IQueryHandler<,>).Assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType && 
                    i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)))
            .Select(i => new { HandlerType = i, ImplementationType = GetImplementationType(i) });

        foreach (var handler in queryHandlerTypes)
        {
            services.AddScoped(handler.HandlerType, handler.ImplementationType);
        }

        // Register all ICommandHandler implementations
        var commandHandlerTypes = typeof(ICommandHandler<>).Assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType && 
                    i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)))
            .Select(i => new { HandlerType = i, ImplementationType = GetImplementationType(i) });

        foreach (var handler in commandHandlerTypes)
        {
            services.AddScoped(handler.HandlerType, handler.ImplementationType);
        }

        return services;
    }

    private static Type GetImplementationType(Type interfaceType)
    {
        var handlerParam = interfaceType.GetGenericArguments()[0];
        var resultParam = interfaceType.GetGenericArguments()[1];

        return typeof(IQueryHandler<,>).Assembly
            .GetTypes()
            .FirstOrDefault(t => t.IsClass && !t.IsAbstract &&
                t.GetInterfaces().Any(i => i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>) &&
                    i.GetGenericArguments()[0] == handlerParam &&
                    i.GetGenericArguments()[1] == resultParam))
            ?? typeof(ICommandHandler<>).Assembly
            .GetTypes()
            .FirstOrDefault(t => t.IsClass && !t.IsAbstract &&
                t.GetInterfaces().Any(i => i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) &&
                    i.GetGenericArguments()[0] == handlerParam &&
                    i.GetGenericArguments()[1] == resultParam))
            ?? throw new InvalidOperationException($"No implementation found for handler interface {interfaceType}");
    }
}
