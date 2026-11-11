using System.Reflection;
using Discount.Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Discount.Application;

public static class ApplicationLayerServices
{
    public static IServiceCollection AddApplicationLayerServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddValidatorsFromAssembly(assembly);
        services.AddMediatR(con =>
        {
            con.RegisterServicesFromAssembly(assembly);
            con.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }
}