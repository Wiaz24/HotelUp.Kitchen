using HotelUp.Kitchen.Persistence.DTOs;
using HotelUp.Kitchen.Persistence.Entities;
using HotelUp.Kitchen.Persistence.Repositories;
using HotelUp.Kitchen.Services.Events;
using HotelUp.Kitchen.Services.Events.DTOs;
using HotelUp.Kitchen.Services.Services.Exceptions;
using MassTransit;

namespace HotelUp.Kitchen.Services.Services;

public class MenuService : IMenuService
{
    private readonly IPublishEndpoint _bus;
    private readonly ICookRepository _cookRepository;
    private readonly IDishRepository _dishRepository;
    private readonly IMenuRepository _menuRepository;

    public MenuService(IMenuRepository menuRepository, IDishRepository dishRepository,
        IPublishEndpoint bus, ICookRepository cookRepository)
    {
        _menuRepository = menuRepository;
        _dishRepository = dishRepository;
        _bus = bus;
        _cookRepository = cookRepository;
    }

    public Task<Menu?> GetPublishedMenuByServingDateAsync(DateOnly servingDate)
    {
        return _menuRepository.GetPublishedMenuByServingDateAsync(servingDate);
    }

    public Task<IEnumerable<Menu>> GetMenusByCookIdAsync(Guid cookId)
    {
        return _menuRepository.GetByCookIdAsync(cookId);
    }

    public async Task CreateAsync(Guid cookId, CreateMenuDto createMenuDto)
    {
        var cook = await _cookRepository.GetByIdAsync(cookId);
        if (cook is null) throw new CookNotFoundException(cookId);
        var existingMenu = await _menuRepository.GetByDateAsync(createMenuDto.ServingDate);
        if (existingMenu is not null) throw new MenuAlreadyExistsException(createMenuDto.ServingDate);

        var dishes = (await _dishRepository
                .GetByNameListAsync(createMenuDto.Dishes))
            .ToList();

        if (dishes.Count != createMenuDto.Dishes.Count)
            throw new DishNotFoundException(createMenuDto.Dishes
                .Except(dishes
                    .Select(d => d.Name))
                .First());

        var menu = new Menu
        {
            ServingDate = createMenuDto.ServingDate,
            Cook = cook,
            Dishes = dishes,
            Published = false
        };

        await _menuRepository.AddAsync(menu);
    }

    public async Task AddDishToMenuAsync(Guid cookId, DateOnly servingDate, string dishName)
    {
        var menu = await _menuRepository.GetByDateAsync(servingDate);
        if (menu is null) throw new MenuNotFoundException(servingDate);

        await ValidateCook(cookId, menu);
        var dish = await _dishRepository.GetByNameAsync(dishName);
        if (dish is null)
        {
            throw new DishNotFoundException(dishName);
        }

        if (menu.Dishes.FirstOrDefault(d => d.Name == dish.Name) is not null)
        {
            throw new DishAlreadyExistsException(dish.Name);
        }

        menu.Dishes.Add(dish);
        await _menuRepository.UpdateAsync(menu);
    }

    public async Task RemoveDishFromMenuAsync(Guid cookId, DateOnly servingDate, string dishName)
    {
        var menu = await _menuRepository.GetByDateAsync(servingDate);
        if (menu is null)
        {
            throw new MenuNotFoundException(servingDate);
        }

        await ValidateCook(cookId, menu);
        if (menu.Dishes.Any(d => d.Name == dishName) is false)
        {
            throw new DishNotFoundException(dishName);
        }

        menu.Dishes.RemoveAll(d => d.Name == dishName);
        await _menuRepository.UpdateAsync(menu);
    }

    public async Task PublishAsync(Guid cookId, DateOnly servingDate)
    {
        var menu = await _menuRepository.GetByDateAsync(servingDate);
        if (menu is null)
        {
            throw new MenuNotFoundException(servingDate);
        }

        await ValidateCook(cookId, menu);
        menu.Published = true;
        await _menuRepository.UpdateAsync(menu);
        var menuPublishedEvent = new MenuPublishedEvent
        {
            ServingDate = menu.ServingDate,
            MenuItems = menu.Dishes.Select(d =>
                new MenuItem
                {
                    Name = d.Name,
                    ImageUrl = d.ImageUrl
                }).ToList()
        };
        await _bus.Publish(menuPublishedEvent);
    }

    private async Task ValidateCook(Guid cookId, Menu menu)
    {
        var cook = await _cookRepository.GetByIdAsync(cookId);
        if (cook is null)
        {
            throw new CookNotFoundException(cookId);
        }

        if (menu.Cook.Id != cookId) throw new MenuCookMismatchException(cookId, menu.Cook.Id);
    }
}