using Ordering.Application.Abstractions;
using Ordering.Application.Mappers;
using Ordering.Core.Repositories;

namespace Ordering.Application.Orders.CreateOrder;

public class CreateOrderHandler(IOrderRepository orderRepository) : IRequestHandler<CreateOrderCommand, int>
{
    public async Task<int> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = command.MapToEntity();
        await orderRepository.AddAsync(order, cancellationToken);
        return order.Id;
    }
}