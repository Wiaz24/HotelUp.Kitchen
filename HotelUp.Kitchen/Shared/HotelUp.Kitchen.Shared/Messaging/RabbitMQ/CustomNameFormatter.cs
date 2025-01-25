using MassTransit;

namespace HotelUp.Kitchen.Shared.Messaging.RabbitMQ;

public class CustomNameFormatter : IEntityNameFormatter
{
    public string FormatEntityName<T>()
    {
        return $"HotelUp.Kitchen:{typeof(T).Name}";
    }
}