using EventBus.Messages.Events;
using MassTransit;

namespace Payment.Api.Consumers;

public class OrderCreatedConsumer(IPublishEndpoint publishEndpoint, ILogger<OrderCreatedConsumer> logger) : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var message = context.Message;
        logger.LogInformation("Processing payment for order ID: {OrderId}", message.Id);
        await Task.Delay(1000);
        if (message.TotalPrice > 0)
        {
            var completedEvent = new PaymentCompletedEvent
            {
                OrderId = message.Id,
                CorrelationId = message.CorrelationId,
            };
            await publishEndpoint.Publish(completedEvent);
            logger.LogInformation("Payment completed for order ID: {OrderId}", message.Id);
        }
        else
        {
            var failedEvent = new PaymentFailedEvent
            {
                OrderId = message.Id,
                CorrelationId =  message.CorrelationId,
                Reason = "Total price was zero or negative"
            };
            await publishEndpoint.Publish(failedEvent);
            logger.LogWarning("Payment failed for order ID: {OrderId}", message.Id);
        }
    }
}