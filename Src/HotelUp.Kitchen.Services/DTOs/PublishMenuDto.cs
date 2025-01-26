using System.ComponentModel;

namespace HotelUp.Kitchen.Services.DTOs;

public record PublishMenuDto
{
    [DefaultValue("2026-01-01")]
    public required DateOnly ServingDate { get; init; }
}