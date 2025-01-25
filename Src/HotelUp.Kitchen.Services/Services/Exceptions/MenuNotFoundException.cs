using HotelUp.Kitchen.Shared.Exceptions;

namespace HotelUp.Kitchen.Services.Services.Exceptions;

public class MenuNotFoundException : BusinessRuleException
{
    public MenuNotFoundException(DateOnly servingDate) : base($"Menu for {servingDate} not found")
    {
    }
}