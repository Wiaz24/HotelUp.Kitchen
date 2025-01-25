using HotelUp.Kitchen.Persistence.DTOs;
using HotelUp.Kitchen.Persistence.Entities;
using HotelUp.Kitchen.Persistence.Repositories;
using HotelUp.Kitchen.Services.Services;
using HotelUp.Kitchen.Services.Services.Exceptions;
using MassTransit;
using NSubstitute;
using Shouldly;

namespace HotelUp.Kitchen.Tests.Unit.Services;

public class MenuServiceTests
{
    private const string ExampleImageUrl = "https://example.com/image.jpg";
    
    [Fact]
    public async Task CreateAsync_WhenCookDoesNotExist_ThrowsCookNotFoundException()
    {
        // Arrange
        var menuRepository = Substitute.For<IMenuRepository>();
        var dishRepository = Substitute.For<IDishRepository>();
        var bus = Substitute.For<IPublishEndpoint>();
        var cookRepository = Substitute.For<ICookRepository>();
        var createMenuDto = new CreateMenuDto
        {
            ServingDate = new DateOnly(2026, 1, 1),
            Dishes = new List<string> { "Pizza", "Lasagne" }
        };

        cookRepository.GetByIdAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult<Cook?>(null));
        var menuService = new MenuService(menuRepository, dishRepository, bus, cookRepository);
        
        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await menuService.CreateAsync(Guid.NewGuid(), createMenuDto));
        
        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<CookNotFoundException>();
    }
    
    [Fact]
    public async Task CreateAsync_WhenMenuAlreadyExists_ThrowsMenuAlreadyExistsException()
    {
        // Arrange
        var menuRepository = Substitute.For<IMenuRepository>();
        var dishRepository = Substitute.For<IDishRepository>();
        var bus = Substitute.For<IPublishEndpoint>();
        var cookRepository = Substitute.For<ICookRepository>();
        var createMenuDto = new CreateMenuDto
        {
            ServingDate = new DateOnly(2026, 1, 1),
            Dishes = ["Pizza", "Lasagne" ]
        };
        var exampleCook = new Cook
        {
            Id = Guid.NewGuid(),
            FoodTasks = []
        };
        var exampleMenu = new Menu
        {
            ServingDate = new DateOnly(2026, 1, 1),
            Cook = exampleCook,
            Dishes = [],
            Published = false
        };

        cookRepository.GetByIdAsync(Arg.Any<Guid>())!
            .Returns(Task.FromResult(exampleCook));
        menuRepository.GetByDateAsync(Arg.Any<DateOnly>())!
            .Returns(Task.FromResult(exampleMenu));
        var menuService = new MenuService(menuRepository, dishRepository, bus, cookRepository);
        
        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await menuService.CreateAsync(Guid.NewGuid(), createMenuDto));
        
        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<MenuAlreadyExistsException>();
    }
    
    [Fact]
    public async Task CreateAsync_WhenDishDoesNotExist_ThrowsDishNotFoundException()
    {
        // Arrange
        var menuRepository = Substitute.For<IMenuRepository>();
        var dishRepository = Substitute.For<IDishRepository>();
        var bus = Substitute.For<IPublishEndpoint>();
        var cookRepository = Substitute.For<ICookRepository>();
        var createMenuDto = new CreateMenuDto
        {
            ServingDate = new DateOnly(2026, 1, 1),
            Dishes = ["Pizza", "Lasagne"]
        };
        var exampleCook = new Cook
        {
            Id = Guid.NewGuid(),
            FoodTasks = []
        };
        var exampleDish = new Dish
        {
            Name = "Pizza",
            Price = 21.37m,
            ImageUrl = ExampleImageUrl
        };

        cookRepository.GetByIdAsync(Arg.Any<Guid>())!
            .Returns(Task.FromResult(exampleCook));
        menuRepository.GetByDateAsync(Arg.Any<DateOnly>())!
            .Returns(Task.FromResult<Menu?>(null));
        dishRepository.GetByNameListAsync(Arg.Any<List<string>>())!
            .Returns(Task.FromResult<IEnumerable<Dish>>(new List<Dish> { exampleDish }));
        var menuService = new MenuService(menuRepository, dishRepository, bus, cookRepository);
        
        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await menuService.CreateAsync(Guid.NewGuid(), createMenuDto));
        
        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<DishNotFoundException>();
    }
    
    [Fact]
    public async Task CreateAsync_WhenDishCountDoesNotMatch_ThrowsDishNotFoundException()
    {
        // Arrange
        var menuRepository = Substitute.For<IMenuRepository>();
        var dishRepository = Substitute.For<IDishRepository>();
        var bus = Substitute.For<IPublishEndpoint>();
        var cookRepository = Substitute.For<ICookRepository>();
        var createMenuDto = new CreateMenuDto
        {
            ServingDate = new DateOnly(2026, 1, 1),
            Dishes = ["Pizza", "Lasagne"]
        };
        var exampleCook = new Cook
        {
            Id = Guid.NewGuid(),
            FoodTasks = []
        };
        var exampleDish = new Dish
        {
            Name = "Pizza",
            Price = 21.37m,
            ImageUrl = ExampleImageUrl
        };

        cookRepository.GetByIdAsync(Arg.Any<Guid>())!
            .Returns(Task.FromResult(exampleCook));
        menuRepository.GetByDateAsync(Arg.Any<DateOnly>())!
            .Returns(Task.FromResult<Menu?>(null));
        dishRepository.GetByNameListAsync(Arg.Any<List<string>>())!
            .Returns(Task.FromResult<IEnumerable<Dish>>(new List<Dish> { exampleDish }));
        var menuService = new MenuService(menuRepository, dishRepository, bus, cookRepository);
        
        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await menuService.CreateAsync(Guid.NewGuid(), createMenuDto));
        
        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<DishNotFoundException>();
    }
    
    [Fact]
    public async Task CreateAsync_WhenAllDataIsValid_CreatesMenu()
    {
        // Arrange
        var menuRepository = Substitute.For<IMenuRepository>();
        var dishRepository = Substitute.For<IDishRepository>();
        var bus = Substitute.For<IPublishEndpoint>();
        var cookRepository = Substitute.For<ICookRepository>();
        var createMenuDto = new CreateMenuDto
        {
            ServingDate = new DateOnly(2026, 1, 1),
            Dishes = ["Pizza", "Lasagne"]
        };
        var exampleCook = new Cook
        {
            Id = Guid.NewGuid(),
            FoodTasks = []
        };
        var exampleDishes = new List<Dish>
        {
            new Dish
            {
                Name = "Pizza",
                Price = 21.37m,
                ImageUrl = ExampleImageUrl
            },
            new Dish
            {
                Name = "Lasagne",
                Price = 18.99m,
                ImageUrl = ExampleImageUrl
            }
        };

        cookRepository.GetByIdAsync(Arg.Any<Guid>())!
            .Returns(Task.FromResult(exampleCook));
        menuRepository.GetByDateAsync(Arg.Any<DateOnly>())!
            .Returns(Task.FromResult<Menu?>(null));
        dishRepository.GetByNameListAsync(Arg.Any<List<string>>())!
            .Returns(Task.FromResult<IEnumerable<Dish>>(exampleDishes));
        var menuService = new MenuService(menuRepository, dishRepository, bus, cookRepository);
        
        // Act
        await menuService.CreateAsync(Guid.NewGuid(), createMenuDto);
        
        // Assert
        await menuRepository.Received().AddAsync(Arg.Any<Menu>());
    }
}