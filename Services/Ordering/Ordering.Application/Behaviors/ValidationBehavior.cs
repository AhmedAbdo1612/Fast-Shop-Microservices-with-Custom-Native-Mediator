using FluentValidation;
using Ordering.Application.Mediator;

namespace Ordering.Application.Behaviors;

public class ValidationBehavior<TRequest, TResult>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResult>
{
    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next,
        CancellationToken cancellationToken)
    {
        if (validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults =
                await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var errors = validationResults.Where(x => x.Errors.Any()).SelectMany(x => x.Errors).ToList();
            if (errors.Any()) throw new ValidationException(errors);
        }

        return await next();
    }
}