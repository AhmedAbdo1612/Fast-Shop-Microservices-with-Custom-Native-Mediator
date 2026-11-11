using Discount.Api.DependencyInjection;
using Discount.Api.Services;
using Discount.Application;
using Discount.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InjectDatabaseServices(builder.Configuration);
builder.Services.AddApplicationLayerServices();
builder.Services.AddGrpc();
var app = builder.Build();
app.MigrateDatabase();

app.MapGrpcService<DiscountService>();
app.MapGet("/",
    async context =>
    {
        await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client...");
    });
app.UseHttpsRedirection();
app.Run();