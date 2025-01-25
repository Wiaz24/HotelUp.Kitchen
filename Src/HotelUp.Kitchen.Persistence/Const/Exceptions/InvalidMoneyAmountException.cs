using HotelUp.Kitchen.Shared.Exceptions;

namespace HotelUp.Kitchen.Persistence.Const.Exceptions;

public class InvalidMoneyAmountException : BusinessRuleException
{
    public InvalidMoneyAmountException(decimal amount) 
        : base($"Invalid money amount: {amount}. Amount must be greater than or equal to 0.")
    {
    }
}