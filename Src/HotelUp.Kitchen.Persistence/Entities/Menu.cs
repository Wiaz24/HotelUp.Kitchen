namespace HotelUp.Kitchen.Persistence.Entities;

public class Menu
{
    public required DateOnly ServingDate { get; init; }
    public required bool Published { get; init; } = false;
    public required List<Dish> Dishes { get; init; }
}