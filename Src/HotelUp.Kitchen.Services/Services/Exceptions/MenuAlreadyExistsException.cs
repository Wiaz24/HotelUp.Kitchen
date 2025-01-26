using HotelUp.Kitchen.Shared.Exceptions;

namespace HotelUp.Kitchen.Services.Services.Exceptions;

public class MenuAlreadyExistsException : BusinessRuleException
{
    public MenuAlreadyExistsException(DateOnly servingDate) : base($"Menu for {servingDate} already exists")
    {
    }
}