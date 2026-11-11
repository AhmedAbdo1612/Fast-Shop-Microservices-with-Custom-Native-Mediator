namespace Ordering.Application.Abstractions;

public interface ICommandHandler<TCommand,TResult> : IRequestHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    Task<TResult> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Unit> where TCommand : ICommand<Unit>
{
    Task Handle(TCommand command, CancellationToken cancellationToken);
}