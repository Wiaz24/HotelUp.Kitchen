namespace HotelUp.Kitchen.API.DTOs;

public record SearchForDishesDto
{
    public required string SearchString { get; init; }
}