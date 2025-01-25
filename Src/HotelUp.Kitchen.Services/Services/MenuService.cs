using HotelUp.Kitchen.Persistence.Entities;
using HotelUp.Kitchen.Persistence.Repositories;
using HotelUp.Kitchen.Services.Events;
using HotelUp.Kitchen.Services.Events.DTOs;
using HotelUp.Kitchen.Services.Services.Exceptions;
using MassTransit;

namespace HotelUp.Kitchen.Services.Services;

public class MenuService : IMenuService
{
    private readonly IMenuRepository _menuRepository;
    private readonly IDishRepository _dishRepository;
    private readonly IPublishEndpoint _bus;

    public MenuService(IMenuRepository menuRepository, IDishRepository dishRepository, IPublishEndpoint bus)
    {
        _menuRepository = menuRepository;
        _dishRepository = dishRepository;
        _bus = bus;
    }

    public Task<Menu?> GetByServingDateAsync(DateOnly servingDate)
    {
        return _menuRepository.GetByServingDateAsync(servingDate);
    }

    public async Task CreateAsync(Menu menu)
    {
        var existingMenu = await _menuRepository.GetByServingDateAsync(menu.ServingDate);
        if (existingMenu is not null)
        {
            throw new MenuAlreadyExistsException(menu.ServingDate);
        }
        await _menuRepository.AddAsync(menu);
    }

    public async Task AddDishToMenuAsync(DateOnly servingDate, string dishName)
    {
        var menu = await _menuRepository.GetByServingDateAsync(servingDate);
        if (menu is null)
        {
            throw new MenuNotFoundException(servingDate);
        }
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

    public async Task RemoveDishFromMenuAsync(DateOnly servingDate, string dishName)
    {
        var menu = await _menuRepository.GetByServingDateAsync(servingDate);
        if (menu is null)
        {
            throw new MenuNotFoundException(servingDate);
        }
        if (menu.Dishes.Any(d => d.Name == dishName) is false)
        {
            throw new DishNotFoundException(dishName);
        }
        menu.Dishes.RemoveAll(d => d.Name == dishName);
        await _menuRepository.UpdateAsync(menu);
    }

    public async Task PublishAsync(DateOnly servingDate)
    {
        var menu = await _menuRepository.GetByServingDateAsync(servingDate);
        if (menu is null)
        {
            throw new MenuNotFoundException(servingDate);
        }
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
}