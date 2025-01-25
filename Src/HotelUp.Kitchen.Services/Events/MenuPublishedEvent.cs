using HotelUp.Kitchen.Services.Events.DTOs;

namespace HotelUp.Kitchen.Services.Events;

public record MenuPublishedEvent
{
    public required DateOnly ServingDate { get; init; }
    public required List<MenuItem> MenuItems { get; init; }
}