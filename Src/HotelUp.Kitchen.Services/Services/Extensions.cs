using Microsoft.Extensions.DependencyInjection;

namespace HotelUp.Kitchen.Services.Services;

public static class Extensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IDishService, DishService>();
        return services;
    }
}