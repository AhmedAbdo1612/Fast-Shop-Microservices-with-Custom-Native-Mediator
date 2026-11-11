namespace Ordering.Application.Abstractions;

public interface ICommand<TResult> :IRequest<TResult>
{
}
public interface ICommand :IRequest;