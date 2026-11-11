using Basket.Application.GrpcService;
using Basket.Application.Settings;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using Basket.Infrastructure.Settings;
using Discount.Grpc.Protos;
using MassTransit;
using Microsoft.Extensions.Options;

namespace Basket.Api;

public static class DependencyInjection
{
    public static IServiceCollection InjectServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        var grpcSettings = configuration.GetSection(nameof(GrpcSettings)).Get<GrpcSettings>();
        services.Configure<GrpcSettings>(configuration.GetSection(nameof(GrpcSettings)));
     
        services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>((sp, con) =>
        {
            var settings = sp.GetRequiredService<IOptions<GrpcSettings>>().Value;
            con.Address = new Uri(settings.DiscountUrl);
        });

        services.AddScoped<DiscountGrpcService>();

        var redisSetting = configuration.GetSection(nameof(CacheSettings)).Get<CacheSettings>();
        services.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = redisSetting!.ConnectionString;
        });
        
        services.AddScoped<IBasketRepository, BasketRepository>();

        services.AddMassTransit(con =>
        {
            con.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["EventBusSettings:HostAddress"]);
            });
        });
        return services;
    }
}