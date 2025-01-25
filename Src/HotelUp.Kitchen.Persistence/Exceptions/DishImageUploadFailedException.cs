using HotelUp.Kitchen.Shared.Exceptions;

namespace HotelUp.Kitchen.Persistence.Exceptions;

public class DishImageUploadFailedException : BusinessRuleException
{
    public DishImageUploadFailedException() : base("Failed to upload an image.")
    {
    }
}