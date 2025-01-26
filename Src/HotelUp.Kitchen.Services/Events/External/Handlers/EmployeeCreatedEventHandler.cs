using HotelUp.Employee.Services.Events;
using HotelUp.Kitchen.Services.Events.DTOs;
using HotelUp.Kitchen.Services.Services;
using MassTransit;

namespace HotelUp.Kitchen.Services.Events.External.Handlers;

public class EmployeeCreatedEventHandler : IConsumer<EmployeeCreatedEvent>
{
    private readonly ICookService _cookService;

    public EmployeeCreatedEventHandler(ICookService cookService)
    {
        _cookService = cookService;
    }

    public Task Consume(ConsumeContext<EmployeeCreatedEvent> context)
    {
        var message = context.Message;
        if (message.Role == EmployeeType.Cook)
        {
            return _cookService.CreateAsync(message.Id);
        }
        else
        {
            return Task.CompletedTask;
        }
    }
}