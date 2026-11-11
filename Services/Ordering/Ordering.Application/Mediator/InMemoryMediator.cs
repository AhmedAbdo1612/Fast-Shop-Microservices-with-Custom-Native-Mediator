using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Abstractions;

namespace Ordering.Application.Mediator;

public interface IMediator
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken =default);
    Task Send(IRequest request, CancellationToken cancellationToken =default);
}

public class InMemoryMediator : IMediator
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly List<Type> _sortedBehaviors;

    public InMemoryMediator([Required] IServiceScopeFactory scopeFactory, Dictionary<int, Type>? behaviors = null)
    {
        _scopeFactory = scopeFactory;
        _sortedBehaviors = new List<Type>();
        if (behaviors != null)
        {
            _sortedBehaviors= behaviors.OrderBy(x => x.Key).Select(x => x.Value).ToList();
        }
    }


  public async Task Send(IRequest request, CancellationToken cancellationToken = default)
{
    if (request is null) throw new ArgumentNullException(nameof(request));

    var requestType = request.GetType();
    var handlerType = typeof(IRequestHandler<>).MakeGenericType(requestType);

    using var scope = _scopeFactory.CreateScope();
    
    var handler = scope.ServiceProvider.GetService(handlerType)
                  ?? throw new InvalidOperationException($"Could not resolve handler {handlerType}");
                  
    var handleMethod = handlerType.GetMethod("Handle") ??
                       throw new InvalidOperationException($"Could not resolve handler {handlerType}");


    RequestHandlerDelegate<Unit> next = async () =>
    {
        var task = (Task)handleMethod.Invoke(handler, new object[] { request, cancellationToken })!;
        await task;
        return Unit.Value; 
    };

    if (_sortedBehaviors.Count > 0)
    {
        var behaviorInterfaceType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, typeof(Unit));

        for (int i = _sortedBehaviors.Count - 1; i >= 0; i--)
        {
            var behaviorType = _sortedBehaviors[i];

            var closedBehaviorType = behaviorType.IsGenericTypeDefinition 
                ? behaviorType.MakeGenericType(requestType, typeof(Unit)) 
                : behaviorType;

            var behaviorInstance = scope.ServiceProvider.GetService(closedBehaviorType);
            if (behaviorInstance == null) continue;

            var behaviorMethod = closedBehaviorType.GetMethod("Handle")!;
            var currentNext = next;

            next = async () =>
            {
                var task = (Task<Unit>)behaviorMethod.Invoke(behaviorInstance,
                    new object[] { request, currentNext, cancellationToken })!;
                return await task;
            };
        }
    }

    await next();
}

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
        using var scope = _scopeFactory.CreateScope();
        var handler = scope.ServiceProvider.GetService(handlerType) ??
                      throw new InvalidOperationException($"Could not find handler for type {requestType.Name}");
        var handleMethod = handlerType.GetMethod("Handle")!;

        RequestHandlerDelegate<TResponse> next = async () =>
        {
            var task = (Task<TResponse>)handleMethod.Invoke(handler, new object[] { request, cancellationToken })!;
            return await task;
        };
        if (_sortedBehaviors.Count > 0)
        {
            for (int i = _sortedBehaviors.Count - 1; i >= 0; i--)
            {
                var behaviorType = _sortedBehaviors[i];
                
                var closedBehaviorType = behaviorType.IsGenericTypeDefinition 
                    ? behaviorType.MakeGenericType(requestType, typeof(TResponse)) 
                    : behaviorType;
                var behaviorInstance = scope.ServiceProvider.GetService(closedBehaviorType);
                if (behaviorInstance == null) continue;
                var behaviorMethod = closedBehaviorType.GetMethod("Handle")!;
                var currentNext = next;
                next = async () =>
                {
                    var task = (Task<TResponse>)behaviorMethod.Invoke(behaviorInstance,
                        new object[] { request, currentNext, cancellationToken })!;
                    return await task;
                };
            }
        }

        return await next();
    }

/*private void PreloadHandlers()
    {
        if (!HandlerCache.IsEmpty) return;
        var assembly = Assembly.GetExecutingAssembly();
        var handlerTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .SelectMany(t => t.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
            .Where(x => x.Interface.IsGenericType
                        && (x.Interface.GetGenericTypeDefinition() == typeof(IRequestHandler<,>) ||
                            x.Interface.GetGenericTypeDefinition() == typeof(IRequestHandler<>))
            );
        foreach (var item in handlerTypes)
        {
            var requestType = item.Interface.GetGenericArguments()[0];
            var handleMethod = item.Type.GetMethod("Handle") ??
                               throw new InvalidOperationException(
                                   $"Could not find handler for type {requestType.Name}");
            var metaData = new HandlerMetadata(item.Interface, handleMethod);
            HandlerCache[requestType] = metaData;
        }
    }*/


    private class HandlerMetadata
    {
        public Type HandlerType { get; }
        public MethodInfo HandleMethod { get; }

        public HandlerMetadata(Type handlerType, MethodInfo handleMethod)
        {
            HandlerType = handlerType;
            HandleMethod = handleMethod;
        }
    }
}