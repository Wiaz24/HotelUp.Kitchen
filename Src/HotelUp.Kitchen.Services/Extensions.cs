using HotelUp.Kitchen.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HotelUp.Kitchen.Services;

public static class Extensions
{
    public static IServiceCollection AddServiceLayer(this IServiceCollection services)
    {
        services.AddApplicationServices();
        return services;
    }
}