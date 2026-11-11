using FluentValidation;
using Ordering.Application.Abstractions;

namespace Ordering.Application.Validation;

/*public class ValidationCommandHandlerDecorator<TCommand, TResult>(
    ICommandHandler<TCommand, TResult> inner,
    IEnumerable<IValidator<TCommand>> validators
) : ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    public async Task<TResult> Handle(TCommand command, CancellationToken cancellationToken)
    {
        ValidatorHelper.Validate(command, validators);
        return await inner.Handle(command, cancellationToken);
    }
}*/


