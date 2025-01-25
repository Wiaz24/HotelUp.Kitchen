using HotelUp.Kitchen.Persistence.EFCore.Health;
using HotelUp.Kitchen.Persistence.EFCore.Postgres;
using Microsoft.Extensions.DependencyInjection;

namespace HotelUp.Kitchen.Persistence.EFCore;

internal static class Extensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.ConfigurePostgres();
        services.AddPostgres<AppDbContext>();
        services.AddHostedService<DatabaseInitializer>();
        services.AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("Database");
        return services;
    }
}