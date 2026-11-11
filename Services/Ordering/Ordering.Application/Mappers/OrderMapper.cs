using System.Text.Json;
using EventBus.Messages.Events;
using Ordering.Application.DTOs;
using Ordering.Application.Orders.CreateOrder;
using Ordering.Core.Entities;
using Mapster;
using Ordering.Application.Constants;
using Ordering.Application.Orders.UpdateOrder;

namespace Ordering.Application.Mappers;

public static class OrderMapper
{
    public static OrderDto MapToDto(this Order o) => new OrderDto(
        o.Id,
        SafeString(o.UserName),
        o.TotalPrice,
        SafeString(o.FirstName),
        SafeString(o.LastName),
        SafeString(o.EmailAddress),
        SafeString(o.AddressLine),
        SafeString(o.Country),
        SafeString(o.State),
        SafeString(o.ZipCode),
        SafeString(o.CardName),
        SafeString(o.CardNumber),
        SafeString(o.Cvv),
        SafeString(o.Expiration),
        o.PaymentMethodId
    );

    public static Order MapToEntity(this CreateOrderCommand c) => c.Adapt<Order>();

    public static void ApplyUpdate(this Order order, UpdateOrderCommand command)
    {
        order.UserName = command.UserName;
        order.TotalPrice = command.TotalPrice;
        order.FirstName = command.FirstName;
        order.LastName = command.LastName;
        order.EmailAddress = command.EmailAddress;
        order.AddressLine = command.AddressLine;
        order.Country = command.Country;
        order.State = command.State;
        order.ZipCode = command.ZipCode;
        order.CardName = command.CardName;
        order.CardNumber = command.CardNumber;
        order.Cvv = command.Cvv;
    }

    public static OutboxMessage MapToOutboxMessage(this Order order, Guid correlationId)
    {
        var outboxMessage = new OutboxMessage
        {
            CorrelationId = correlationId.ToString(),
            Type = OutboxMessageTypes.OrderCreated,
            OccurredOn = DateTime.UtcNow,
            Content = JsonSerializer.Serialize(new
            {
                order.Id,
                order.UserName,
                order.TotalPrice,
                order.FirstName,
                order.LastName,
                order.EmailAddress,
                order.AddressLine,
                order.Country,
                order.State,
                order.ZipCode,
                order.CardName,
                order.CardNumber,
                order.Cvv,
                order.PaymentMethodId,
                order.Status
            })
        };
        return outboxMessage;
    }

    public static CreateOrderCommand ToCheckoutOrderCommand(this BasketCheckoutEvent evt)
    {
        return evt.Adapt<CreateOrderCommand>();
    }

    private static string SafeString(string? value) => value ?? string.Empty;
}