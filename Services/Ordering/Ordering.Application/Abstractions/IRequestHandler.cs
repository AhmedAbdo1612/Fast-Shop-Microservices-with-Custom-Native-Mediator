namespace Ordering.Application.Abstractions;

public interface IRequestHandler<in TRequest, TResponse> where  TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest command, CancellationToken cancellationToken = default);
}
public interface IRequestHandler<in TRequest> where  TRequest : IRequest
{
    Task Handle(TRequest command, CancellationToken cancellationToken = default);
}