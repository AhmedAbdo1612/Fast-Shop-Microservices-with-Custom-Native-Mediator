using System.Reflection;
using EventBus.Messages.Common;
using EventBus.Messages.Events;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Behaviors;
using Ordering.Application.Discpatcher;
using Ordering.Application.EventBusConsumer;
using Ordering.Application.Mediator;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOrderingService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderDbContext>(options => options.UseNpgsql(
            configuration.GetConnectionString("OrderDbConnectionString"),
            opt => { opt.EnableRetryOnFailure(3, TimeSpan.FromSeconds(10), null); }));

        services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddHostedService<OutboxMessageDiscpatcher>();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddCustomMediator(con =>
        {
            con.AddBehavior(typeof(ValidationBehavior<,>))
                .AddBehavior(typeof(LoggingBehavior<,>));
        });

        //Masstransit 
        services.AddMassTransit(cfg =>
        {
            cfg.AddConsumer<BasketOrderingConsumer>();
            cfg.AddConsumer<PaymentCompletedConsumer>();
            cfg.AddConsumer<PaymnetFailedConsumer>();
            cfg.UsingRabbitMq((ctx, config) =>
            {
                config.Host(configuration["EventBusSettings:HostAddress"]);
                config.ConfigureEndpoints(ctx);
                config.ReceiveEndpoint(EventBusConstant.PaymentCompletedQueue,
                    c => { c.ConfigureConsumer<PaymentCompletedConsumer>(ctx); });
                config.ReceiveEndpoint(EventBusConstant.PaymentFailedQueue,
                    c => { c.ConfigureConsumer<PaymnetFailedConsumer>(ctx); });
            });
        });

        return services;
    }
}