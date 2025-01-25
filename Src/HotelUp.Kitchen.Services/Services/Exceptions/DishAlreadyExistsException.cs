using HotelUp.Kitchen.Shared.Exceptions;

namespace HotelUp.Kitchen.Services.Services.Exceptions;

public class DishAlreadyExistsException : BusinessRuleException
{
    public DishAlreadyExistsException(string name) : base($"Dish with name '{name}' already exists.")
    {
    }
}