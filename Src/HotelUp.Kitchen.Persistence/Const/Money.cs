using System.Text.Json;
using HotelUp.Kitchen.Persistence.Const.Exceptions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HotelUp.Kitchen.Persistence.Const;

public record Money
{
    public decimal Amount { get; init; }
    public string Currency { get; init; }

    public Money(decimal amount, string currency = "PLN")
    {
        if (amount < 0)
        {
            throw new InvalidMoneyAmountException(amount);
        }
        Amount = amount;
        Currency = currency;
    }
    
    public static implicit operator decimal(Money money) => money.Amount;
    public static implicit operator Money(decimal amount) => new Money(amount);
    
    public override string ToString() => $"{Amount} {Currency}";
    public static ValueConverter GetValueConverter()
    {
        return new ValueConverter<Money, string>(
            v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
            v => JsonSerializer.Deserialize<Money>(v, JsonSerializerOptions.Default)!);
    }

    public static Money operator+ (Money money1, Money money2)
    {
        if (money1.Currency != money2.Currency)
        {
            throw new OperationOnDiffrentCurrenciesException();
        }
        return new Money(money1.Amount + money2.Amount, money1.Currency);
    }
}