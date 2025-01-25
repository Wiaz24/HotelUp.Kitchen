using HotelUp.Kitchen.Persistence.EF.Health;
using HotelUp.Kitchen.Persistence.EF.Postgres;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelUp.Kitchen.Persistence.EF;

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