using System.Reflection;
using Catalog.Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Application;

public static class DependencyInjection
{
    public static IServiceCollection InjectApplicationLayerServices(
        this IServiceCollection services)
    {
        var assemply = Assembly.GetExecutingAssembly();
        services.AddValidatorsFromAssembly(assemply);
        services.AddMediatR(con =>
        {
            con.RegisterServicesFromAssembly(assemply);
            con.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
      
        return services;
    }
}