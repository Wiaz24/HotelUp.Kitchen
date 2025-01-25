using HotelUp.Kitchen.Persistence.EFCore.Config;
using HotelUp.Kitchen.Persistence.EFCore.Postgres;
using HotelUp.Kitchen.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HotelUp.Kitchen.Persistence.EFCore;

public class AppDbContext : DbContext
{
    public DbSet<Cook> Cooks { get; set; }
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<FoodTask> FoodTasks { get; set; }
    public DbSet<Menu> Menus { get; set; }
    
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
        modelBuilder.ApplyConfiguration<Cook>(configuration);
        modelBuilder.ApplyConfiguration<Dish>(configuration);
        modelBuilder.ApplyConfiguration<FoodTask>(configuration);
        modelBuilder.ApplyConfiguration<Menu>(configuration);

        base.OnModelCreating(modelBuilder);
    }
}