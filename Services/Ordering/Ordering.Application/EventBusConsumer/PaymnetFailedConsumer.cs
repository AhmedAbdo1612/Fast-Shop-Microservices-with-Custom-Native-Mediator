using EventBus.Messages.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.EventBusConsumer;

public class PaymnetFailedConsumer(IOrderRepository orderRepository, ILogger<PaymnetFailedConsumer> logger):IConsumer<PaymentFailedEvent>
{
    public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
    {
        var order = await orderRepository.GetByIdAsync(context.Message.OrderId);
        if (order == null)
        {
            logger.LogError($"Order with id {context.Message.OrderId} not found");
            return;
        }

        order.Status = OrderStatus.Failed;
        await orderRepository.UpdateAsync(order);
        logger.LogWarning($"Order with id {context.Message.OrderId} failed");
        
    }
}