namespace HotelUp.Kitchen.Services.DTOs;

public record SearchForDishesDto
{
    public required string SearchString { get; init; }
}