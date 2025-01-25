namespace HotelUp.Kitchen.Persistence.DTOs;

public record SearchForDishesDto
{
    public required string SearchString { get; init; }
}