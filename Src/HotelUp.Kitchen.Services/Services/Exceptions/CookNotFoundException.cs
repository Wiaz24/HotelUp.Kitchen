using HotelUp.Kitchen.Shared.Exceptions;

namespace HotelUp.Kitchen.Services.Services.Exceptions;

public class CookNotFoundException : NotFoundException
{
    public CookNotFoundException(Guid cookId) : base($"Cook with id {cookId} was not found.")
    {
    }
}