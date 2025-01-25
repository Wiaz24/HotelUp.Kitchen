using System.Text.Json.Serialization;
using HotelUp.Kitchen.API.Cors;
using HotelUp.Kitchen.API.Swagger;
using HotelUp.Kitchen.Persistence;
using HotelUp.Kitchen.Services;
using HotelUp.Kitchen.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.AddShared();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddCustomSwagger(builder.Configuration);
builder.Services.AddCorsForFrontend(builder.Configuration);
builder.Services.AddServiceLayer();
builder.Services.AddPersistenceLayer();

var app = builder.Build();

app.UseShared();
app.UseCustomSwagger();
app.UseCorsForFrontend();
app.MapControllers();
app.MapGet("/", () => Results.Redirect("/api/kitchen/swagger/index.html"))
    .Produces(200)
    .ExcludeFromDescription();
app.Run();

public interface IApiMarker
{
}