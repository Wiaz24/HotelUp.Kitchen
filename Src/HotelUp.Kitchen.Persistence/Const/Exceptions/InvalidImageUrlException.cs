using HotelUp.Kitchen.Shared.Exceptions;

namespace HotelUp.Kitchen.Persistence.Const.Exceptions;

public class InvalidImageUrlException : BusinessRuleException
{
    public InvalidImageUrlException(string message) : base($"Image URL is invalid: {message}") { } 
}