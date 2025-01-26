using System.ComponentModel;

namespace HotelUp.Kitchen.Services.DTOs;

public record CreateMenuDto
{
    [DefaultValue("2026-01-01")]
    public required DateOnly ServingDate { get; init; }
    public required List<string> Dishes { get; init; }
}