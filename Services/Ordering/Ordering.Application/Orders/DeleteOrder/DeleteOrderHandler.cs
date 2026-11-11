using Ordering.Application.Abstractions;
using Ordering.Core.Repositories;

namespace Ordering.Application.Orders.DeleteOrder;

public class DeleteOrderHandler (IOrderRepository orderRepository) : IRequestHandler<DeleteOrderCommand>
{
    public async Task Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(command.Id, cancellationToken);
        if (order is null) throw new Exception($"Order with id {command.Id} not found");
        await orderRepository.DeleteAsync(order, cancellationToken);
    }
    
}