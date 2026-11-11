using EventBus.Messages.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Ordering.Application.Mappers;
using Ordering.Application.Mediator;
using Ordering.Application.Orders.CreateOrder;

namespace Ordering.Application.EventBusConsumer;

public class BasketOrderingConsumer(IMediator mediator, ILogger<BasketOrderingConsumer> logger) : IConsumer<BasketCheckoutEvent>
{
    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        using var scope = logger.BeginScope("Consuming basket checkout event for {CorrelationId}", context.CorrelationId);
        CreateOrderCommand command = context.Message.ToCheckoutOrderCommand();
        var result = await mediator.Send(command);
        logger.LogInformation("Basket checkout event for {CorrelationId} completed successfully with order id {result}!", context.CorrelationId, result);
    }
}