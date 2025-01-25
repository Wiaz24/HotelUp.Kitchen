using HotelUp.Kitchen.Persistence.EF.Config;
using HotelUp.Kitchen.Persistence.EF.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HotelUp.Kitchen.Persistence.EF;

public class AppDbContext : DbContext
{
    // public DbSet<Entity> Entities { get; set; }
    private readonly PostgresOptions _postgresOptions;

    public AppDbContext(DbContextOptions<AppDbContext> options, IOptions<PostgresOptions> postgresOptions)
        : base(options)
    {
        _postgresOptions = postgresOptions.Value;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(_postgresOptions.SchemaName);

        var configuration = new DbContextConfiguration();
        // modelBuilder.ApplyConfiguration<Entity>(configuration);

        base.OnModelCreating(modelBuilder);
    }
}