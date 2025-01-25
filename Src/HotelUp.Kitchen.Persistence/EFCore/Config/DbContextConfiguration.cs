using HotelUp.Kitchen.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelUp.Kitchen.Persistence.EFCore.Config;

internal sealed class DbContextConfiguration
    : IEntityTypeConfiguration<Cook>,
        IEntityTypeConfiguration<Dish>,
        IEntityTypeConfiguration<FoodTask>,
        IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Cook> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.FoodTasks)
            .WithOne();
        
        builder.HasMany(x => x.Menus)
            .WithOne();
    }

    public void Configure(EntityTypeBuilder<Dish> builder)
    {
        builder.HasKey(x => x.Name);

        builder.ComplexProperty(x => x.Price);

        builder.ComplexProperty(x => x.ImageUrl);
    }

    public void Configure(EntityTypeBuilder<FoodTask> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Status);
        
        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasConversion(
                x => DateTime.SpecifyKind(x, DateTimeKind.Utc),
                x => DateTime.SpecifyKind(x, DateTimeKind.Utc));
        
        builder.Property(x => x.RoomNumber);
        
        builder.HasMany(x => x.Dishes)
            .WithMany();
        
        builder.OwnsOne(x => x.Reservation, b =>
        {
            b.WithOwner();
            
            b.Property(x => x.ReservationId);
            b.HasIndex(x => x.ReservationId);
            
            b.Property(x => x.StartDate);
            
            b.Property(x => x.EndDate);

            b.PrimitiveCollection(x => x.RoomNumbers);
            
            b.Property(x => x.StartDate)
                .IsRequired()
                .HasConversion(
                    x => DateTime.SpecifyKind(x, DateTimeKind.Utc),
                    x => DateTime.SpecifyKind(x, DateTimeKind.Utc));
        
            b.Property(x => x.EndDate)
                .IsRequired()
                .HasConversion(
                    x => DateTime.SpecifyKind(x, DateTimeKind.Utc),
                    x => DateTime.SpecifyKind(x, DateTimeKind.Utc));
        });
    }

    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.HasKey(x => x.ServingDate);

        builder.Property(x => x.Published);

        builder.HasMany(x => x.Dishes)
            .WithMany();
    }
}