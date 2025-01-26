using HotelUp.Kitchen.Persistence.Entities;
using HotelUp.Kitchen.Services.DTOs;

namespace HotelUp.Kitchen.Services.Services;

public interface IMenuService
{
    Task<Menu?> GetPublishedMenuByServingDateAsync(DateOnly servingDate);
    Task<IEnumerable<Menu>> GetMenusByCookIdAsync(Guid cookId);
    Task CreateAsync(Guid cookId, CreateMenuDto createMenuDto);
    Task AddDishToMenuAsync(Guid cookId, DateOnly servingDate, string dishName);
    Task RemoveDishFromMenuAsync(Guid cookId, DateOnly servingDate, string dishName);
    Task PublishAsync(Guid cookId, DateOnly servingDate);
}