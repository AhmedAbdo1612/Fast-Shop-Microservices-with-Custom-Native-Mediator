using Ordering.Application.Abstractions;
using Ordering.Application.Mappers;
using Ordering.Core.Repositories;

namespace Ordering.Application.Orders.UpdateOrder;

public class UpdateOrderHandler(IOrderRepository orderRepository) : ICommandHandler<UpdateOrderCommand,bool>
{
    public async Task<bool> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        var orderToUpdate = await orderRepository.GetByIdAsync(command.Id,cancellationToken);
        if (orderToUpdate is null) throw new Exception($"Order not found with id {command.Id}");
        orderToUpdate.ApplyUpdate(command);
        await orderRepository.UpdateAsync(orderToUpdate, cancellationToken);
        return true;
    }
}