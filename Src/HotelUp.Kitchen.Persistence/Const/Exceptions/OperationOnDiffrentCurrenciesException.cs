using HotelUp.Kitchen.Shared.Exceptions;

namespace HotelUp.Kitchen.Persistence.Const.Exceptions;

public class OperationOnDiffrentCurrenciesException : BusinessRuleException
{
    public OperationOnDiffrentCurrenciesException() : base("Cannot perform operation on different currencies")
    {
    }
}