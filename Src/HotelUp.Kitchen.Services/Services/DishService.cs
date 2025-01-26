using HotelUp.Kitchen.Persistence.Entities;
using HotelUp.Kitchen.Persistence.Repositories;
using HotelUp.Kitchen.Services.Services.Exceptions;
using Microsoft.AspNetCore.Http;

namespace HotelUp.Kitchen.Services.Services;

public class DishService : IDishService
{
    private readonly IDishImageRepository _dishImageRepository;
    private readonly IDishRepository _dishRepository;

    public DishService(IDishRepository dishRepository, IDishImageRepository dishImageRepository)
    {
        _dishRepository = dishRepository;
        _dishImageRepository = dishImageRepository;
    }

    public Task<IEnumerable<Dish>> SearchDishByNameAsync(string searchString)
    {
        return _dishRepository.SearchByNameAsync(searchString);
    }

    public async Task CreateDishAsync(string name, decimal price, IFormFile image)
    {
        var existingDish = await _dishRepository.GetByNameAsync(name);
        if (existingDish != null) throw new DishAlreadyExistsException(name);
        var url = await _dishImageRepository.UploadImageAsync(image);
        var dish = new Dish
        {
            Name = name,
            Price = price,
            ImageUrl = url
        };
        await _dishRepository.AddAsync(dish);
    }
}