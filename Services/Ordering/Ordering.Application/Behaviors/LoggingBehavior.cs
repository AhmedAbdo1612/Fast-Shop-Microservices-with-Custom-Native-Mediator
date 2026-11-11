using Microsoft.Extensions.Logging;
using Ordering.Application.Mediator;

namespace Ordering.Application.Behaviors;

public class LoggingBehavior<TRequest, TResult>(ILogger<LoggingBehavior<TRequest, TResult>> logger)
    : IPipelineBehavior<TRequest, TResult>
{
    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling request {@Request}", request);
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var result = await next();
        logger.LogInformation("Completed request: {RequestName} in {ElapsedMilliseconds}ms",
            typeof(TRequest).Name, stopwatch.ElapsedMilliseconds);
        return result;
    }
}