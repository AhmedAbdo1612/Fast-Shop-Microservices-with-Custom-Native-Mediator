using System.Reflection;
using Catalog.API.DependencyInjection;
using Catalog.Application;
using Catalog.Infrastructure.Data.SeedData;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddMongoDatabaseServices(builder.Configuration);

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
builder.Services.InjectApplicationLayerServices();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    using var scope = app.Services.CreateScope();
    var mongoDbSettings = scope.ServiceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
    await DatabaseSeeder.SeedAsync(database, mongoDbSettings);
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();