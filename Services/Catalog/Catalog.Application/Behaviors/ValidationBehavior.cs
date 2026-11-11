using FluentValidation;
using MediatR;

namespace Catalog.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
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