using FluentValidation;

namespace Ordering.Application.Validation;

public static class ValidatorHelper
{
    public static void Validate<TRequest>(TRequest request, IEnumerable<IValidator<TRequest>> validators)
    {
        if (validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }
        }
    }
}