using HotelUp.Kitchen.Shared.Exceptions;

namespace HotelUp.Kitchen.Services.Services.Exceptions;

public class MenuNotFoundException : NotFoundException
{
    public MenuNotFoundException(DateOnly servingDate) : base($"Menu for {servingDate} not found")
    {
    }
}