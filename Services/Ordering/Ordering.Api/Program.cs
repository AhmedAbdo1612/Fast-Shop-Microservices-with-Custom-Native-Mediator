using Ordering.Api.Extensions;
using Ordering.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOrderingService(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
var app = builder.Build();
/*app.MigrateDatabase<OrderDbContext>((context, services) =>
{
    var logger = services.GetRequiredService<ILogger<OrderDbSeed>>();
    OrderDbSeed.Seed(context, logger).Wait();
});*/

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();