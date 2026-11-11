using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Abstractions;

namespace Ordering.Application.Mediator;

public class MediatorOptions
{
    internal Dictionary<int, Type> Behaviors { get; } = new();
    private int _order = 0;

    public MediatorOptions AddBehavior(Type behaviorType)
    {
        Behaviors.Add(_order++, behaviorType);
        return this;
    }

    public MediatorOptions AddBehavior<TBehavior>()
    {
        Behaviors.Add(_order++, typeof(TBehavior));
        return this;
    }
}

public static class MediatorExtensions
{
    public static IServiceCollection AddCustomMediator(this IServiceCollection services,
        Action<MediatorOptions>? configurations = null)
    {
       var behaviors= RegisterBehavios(services, configurations);
   
        var assembly = Assembly.GetExecutingAssembly();
        var handlerTypes = assembly!.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .SelectMany(t => t.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
            .Where(x => x.Interface.IsGenericType &&
                        (x.Interface.GetGenericTypeDefinition() == typeof(IRequestHandler<,>) ||
                         x.Interface.GetGenericTypeDefinition() == typeof(IRequestHandler<>))
            );
        foreach (var handler in handlerTypes)
        {
            services.AddTransient(handler.Interface, handler.Type);
        }

        services.AddSingleton<IMediator>(sp =>
        {
            var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
            return new InMemoryMediator(scopeFactory,behaviors.Behaviors);
        });
        return services;
    }

    private static MediatorOptions RegisterBehavios(IServiceCollection services, Action<MediatorOptions>? configurations = null)
    {
        var options = new MediatorOptions();
        configurations?.Invoke(options);
        foreach (var behaviorType in options.Behaviors.Values)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), behaviorType);
            services.AddTransient(behaviorType);
        }

        return options;
    }
}