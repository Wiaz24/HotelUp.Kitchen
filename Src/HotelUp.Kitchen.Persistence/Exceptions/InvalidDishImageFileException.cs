using FluentValidation.Results;
using HotelUp.Kitchen.Shared.Exceptions;

namespace HotelUp.Kitchen.Persistence.Exceptions;

public class InvalidDishImageFileException : BusinessRuleException
{
    public InvalidDishImageFileException(IEnumerable<ValidationFailure> errors) : base("Invalid dish image file.")
    {
        Errors = errors;
    }

    public IEnumerable<ValidationFailure> Errors { get; }
}