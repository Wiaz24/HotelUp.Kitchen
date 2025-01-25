using HotelUp.Kitchen.Persistence.Entities;
using Microsoft.AspNetCore.Http;

namespace HotelUp.Kitchen.Services.Services;

public interface IDishService
{
    Task<IEnumerable<Dish>> SearchDishByNameAsync(string searchString);
    Task CreateDishAsync(string name, decimal price, IFormFile image);
}