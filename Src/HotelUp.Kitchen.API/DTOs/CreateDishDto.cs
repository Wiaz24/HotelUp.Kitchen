using System.ComponentModel;

namespace HotelUp.Kitchen.API.DTOs;

public record CreateDishDto
{
    [DefaultValue("Pizza")]
    public required string Name { get; init; }
    
    [DefaultValue("21,37")]
    public required decimal Price { get; init; }
    public required IFormFile Image { get; init; }
}