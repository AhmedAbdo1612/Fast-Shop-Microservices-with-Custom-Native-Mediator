using System.Text.Json;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ordering.Infrastructure.Data;

namespace Ordering.Application.Discpatcher;

public class OutboxMessageDiscpatcher(IServiceProvider serviceProvider, ILogger<OutboxMessageDiscpatcher> logger) :BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
            var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
            var pendingMessages = await  dbContext.OutboxMessages
                .Where(x=>x.ProcessedOn == null)
                .OrderBy(x=>x.OccurredOn)
                .Take(20)
                .ToListAsync(stoppingToken);
            foreach (var message in pendingMessages)
            {
                try
                {
                    var orderCreatedEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(message.Content);
                    await publishEndpoint.Publish(orderCreatedEvent);
                    message.ProcessedOn = DateTime.UtcNow;
                    logger.LogInformation($"Published outbox message {orderCreatedEvent.Id}");
                }
                catch (Exception e)
                {
                  logger.LogError(e, "Error publishing outbox message");
                }
                await dbContext.SaveChangesAsync(stoppingToken);
                await Task.Delay(5000, stoppingToken);
            }
            
        }
    }
}