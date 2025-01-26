using HotelUp.Employee.Services.Events;
using HotelUp.Kitchen.Persistence.Entities;
using HotelUp.Kitchen.Persistence.Repositories;
using HotelUp.Kitchen.Services.Events.DTOs;
using HotelUp.Kitchen.Services.Services;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit.Abstractions;

namespace HotelUp.Kitchen.Tests.Integration.EventHandlers;

[Collection(nameof(EmployeeCreatedEventHandlerTests))]
public class EmployeeCreatedEventHandlerTests : IntegrationTestsBase
{
    private readonly IBus _bus;
    
    public EmployeeCreatedEventHandlerTests(TestWebAppFactory factory, ITestOutputHelper testOutputHelper) 
        : base(factory, testOutputHelper)
    {
        _bus = ServiceProvider.GetRequiredService<IBus>();
    }
    
    private async Task<Cook?> GetCookAsync(Guid employeeId)
    {
        using var scope = ServiceProvider.CreateScope();
        var cookService = scope.ServiceProvider.GetRequiredService<ICookRepository>();
        return await cookService.GetByIdAsync(employeeId);
    }
    
    [Fact]
    public async Task HandleAsync_WhenEmployeeIsCook_ShouldCreateCook()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var employeeCreatedEvent = new EmployeeCreatedEvent
        {
            Id = employeeId,
            Role = EmployeeType.Cook
        };
        
        // Act
        await _bus.Publish(employeeCreatedEvent);
        await Task.Delay(500);
        
        
        // Assert
        var cook = await GetCookAsync(employeeId);
        cook.ShouldNotBeNull();
        cook.Id.ShouldBe(employeeId);
    }
}