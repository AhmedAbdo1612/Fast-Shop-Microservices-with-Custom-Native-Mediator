using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Basket.Application;

public static class ApplicationLayerDependencyInjection
{
    public static IServiceCollection AddApplicationLayerServices(this IServiceCollection services)
    {
        services.AddMediatR(con => { con.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); });
        return services;
    }
}