namespace HotelUp.Kitchen.Services.Events.DTOs;

public record MenuItem
{
    public required string Name { get; init; }
    public required string ImageUrl { get; init; }
}