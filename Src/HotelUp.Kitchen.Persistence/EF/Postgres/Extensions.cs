using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;

namespace HotelUp.Kitchen.Persistence.EF.Postgres;

public static class Extensions
{
    private const string SectionName = "Postgres";

    public static IServiceCollection ConfigurePostgres(this IServiceCollection services)
    {
        services.AddOptions<PostgresOptions>()
            .BindConfiguration(SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }

    public static IServiceCollection AddPostgres<T>(this IServiceCollection services) where T : DbContext
    {
        var options = services.BuildServiceProvider().GetRequiredService<IOptions<PostgresOptions>>();
        var schemaName = options.Value.SchemaName;
        var connectionString = options.Value.ConnectionString;

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        var dataSource = dataSourceBuilder.Build();
        services.AddDbContext<T>(x => x.UseNpgsql(dataSource,
            builder => { builder.MigrationsHistoryTable("__EFMigrationsHistory", schemaName); }));
        return services;
    }
}