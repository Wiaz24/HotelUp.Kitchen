using System.ComponentModel;

namespace HotelUp.Kitchen.Persistence.DTOs;

public record PublishMenuDto
{
    [DefaultValue("2026-01-01")]
    public required DateOnly ServingDate { get; init; }
}