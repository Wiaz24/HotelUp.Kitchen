using System.Net;
using System.Security.Claims;
using HotelUp.Kitchen.Persistence.EFCore;
using HotelUp.Kitchen.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit.Abstractions;

namespace HotelUp.Kitchen.Tests.Integration.Controllers;

[Collection(nameof(MenuControllerTests))]
public class MenuControllerTests : IntegrationTestsBase
{
    private const string Prefix = "api/kitchen/menu";

    public MenuControllerTests(TestWebAppFactory factory, ITestOutputHelper testOutputHelper)
        : base(factory, testOutputHelper)
    {
    }

    private async Task<Cook> CreateSampleCook()
    {
        using var scope = ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var cook = new Cook
        {
            Id = Guid.NewGuid(),
            FoodTasks = []
        };
        dbContext.Cooks.Add(cook);
        await dbContext.SaveChangesAsync();
        return cook;
    }

    private async Task<Dish> CreateExampleDish()
    {
        using var scope = ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var dish = new Dish
        {
            Name = "Lasagne",
            Price = 21.37m,
            ImageUrl = "https://example.com/lasagne.jpg"
        };
        dbContext.Dishes.Add(dish);
        await dbContext.SaveChangesAsync();
        return dish;
    }

    private async Task<Menu> CreateExampleMenu(IEnumerable<string> dishes, DateOnly servingDate, Guid cookId)
    {
        using var scope = ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var cook = await dbContext.Cooks.FindAsync(cookId);
        cook.ShouldNotBeNull();

        var dishEntities = await dbContext.Dishes.Where(d => dishes.Contains(d.Name)).ToListAsync();
        var menu = new Menu
        {
            Cook = cook,
            Dishes = dishEntities,
            ServingDate = servingDate,
            Published = false
        };
        dbContext.Menus.Add(menu);
        await dbContext.SaveChangesAsync();
        return menu;
    }

    [Fact]
    public async Task PublishMenu_WhenUserIsNotMenuOwner_ShouldReturnException()
    {
        // Arrange
        var cook1 = await CreateSampleCook();
        var cook2 = await CreateSampleCook();
        var cookClaim = new Claim(ClaimTypes.Role, "Cooks");
        var httpClient = CreateHttpClientWithToken(cook1.Id, [cookClaim]);
        var dish = await CreateExampleDish();

        var menu = await CreateExampleMenu([dish.Name], DateOnly.FromDateTime(DateTime.UtcNow), cook2.Id);

        // Act
        var query = $"servingDate={menu.ServingDate.ToString("O")}";
        var response = await httpClient.PutAsync($"{Prefix}/publish?{query}", null);

        // Assert
        var content = await response.Content.ReadAsStringAsync();
        TestOutputHelper.WriteLine(content);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}