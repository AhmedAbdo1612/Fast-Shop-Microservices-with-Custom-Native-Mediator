using EventBus.Messages.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.EventBusConsumer;

public class PaymentCompletedConsumer(IOrderRepository orderRepository, ILogger<PaymentCompletedConsumer> logger):IConsumer<PaymentCompletedEvent>
{
    public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
    {
        var order = await orderRepository.GetByIdAsync(context.Message.OrderId);
        if (order is null)
        {
            logger.LogWarning($"Order with id {context.Message.OrderId} not found");
            return;
        }

        order.Status = OrderStatus.Paid;
        await orderRepository.UpdateAsync(order);
        logger.LogInformation($"Order with id {context.Message.OrderId} paid");
    }
}