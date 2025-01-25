using Microsoft.Extensions.DependencyInjection;

namespace HotelUp.Kitchen.Persistence.Repositories;

public static class Extensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICookRepository, CookRepository>();
        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<IDishRepository, DishRepository>();
        services.AddScoped<IDishImageRepository, S3DishImageRepository>();
        return services;
    }
}