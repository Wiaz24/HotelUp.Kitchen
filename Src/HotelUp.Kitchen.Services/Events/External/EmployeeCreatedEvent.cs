using HotelUp.Kitchen.Services.Events.DTOs;


// ReSharper disable once CheckNamespace
namespace HotelUp.Employee.Services.Events;

public record EmployeeCreatedEvent
{
    public required Guid Id { get; init; }
    public required EmployeeType Role { get; init; }
}