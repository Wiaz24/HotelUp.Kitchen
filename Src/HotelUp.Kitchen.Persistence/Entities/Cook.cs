namespace HotelUp.Kitchen.Persistence.Entities;

public class Cook
{
    public required Guid Id { get; init; }
    public required List<FoodTask> FoodTasks { get; init; }
}