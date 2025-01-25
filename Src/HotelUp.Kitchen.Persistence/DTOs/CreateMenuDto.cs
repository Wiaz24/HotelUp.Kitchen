using System.ComponentModel;

namespace HotelUp.Kitchen.Persistence.DTOs;

public record CreateMenuDto
{
    [DefaultValue("2026-01-01")]
    public required DateOnly ServingDate { get; init; }
    
    // [DefaultValue(typeof(List<string>), "pizza", "pasta")]
    public required List<string> Dishes { get; init; }
}