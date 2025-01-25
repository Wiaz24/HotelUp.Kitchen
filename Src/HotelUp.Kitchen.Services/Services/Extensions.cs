using Microsoft.Extensions.DependencyInjection;

namespace HotelUp.Kitchen.Services.Services;

public static class Extensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Add services here as scoped
        return services;
    }
}