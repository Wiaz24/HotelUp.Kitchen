using HotelUp.Kitchen.Shared.Exceptions;

namespace HotelUp.Kitchen.Services.Services.Exceptions;

public class MenuCookMismatchException : BusinessRuleException
{
    public MenuCookMismatchException(Guid cookId, Guid id)
        : base($"Cook with id {cookId} does not match menu cook with id {id}")
    {
    }
}