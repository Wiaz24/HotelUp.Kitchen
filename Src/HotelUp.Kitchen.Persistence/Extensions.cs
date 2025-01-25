using HotelUp.Kitchen.Persistence.EFCore;
using HotelUp.Kitchen.Persistence.Repositories;
using HotelUp.Kitchen.Persistence.S3;
using Microsoft.Extensions.DependencyInjection;

namespace HotelUp.Kitchen.Persistence;

public static class Extensions
{
    public static IServiceCollection AddPersistenceLayer(this IServiceCollection services)
    {
        services.AddDatabase();
        services.AddRepositories();
        services.AddS3();
        return services;
    }
}