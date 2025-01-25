using HotelUp.Kitchen.Persistence.EF;
using HotelUp.Kitchen.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelUp.Kitchen.Persistence;

public static class Extensions
{
    public static IServiceCollection AddPersistenceLayer(this IServiceCollection services)
    {
        services.AddDatabase();
        services.AddRepositories();
        return services;
    }
}