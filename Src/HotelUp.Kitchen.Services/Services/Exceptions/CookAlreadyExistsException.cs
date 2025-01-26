using HotelUp.Kitchen.Shared.Exceptions;

namespace HotelUp.Kitchen.Services.Services.Exceptions;

public class CookAlreadyExistsException : BusinessRuleException
{
    public CookAlreadyExistsException(Guid id) : base($"Cook with id {id} already exists.")
    {
    }
}