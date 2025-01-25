using HotelUp.Kitchen.Persistence.Entities;

namespace HotelUp.Kitchen.Services.Services;

public interface IMenuService
{
    Task<Menu?> GetByServingDateAsync(DateOnly servingDate);
    Task CreateAsync(Menu menu);
    Task AddDishToMenuAsync(DateOnly servingDate, string dishName);
    Task RemoveDishFromMenuAsync(DateOnly servingDate, string dishName);
    Task PublishAsync(DateOnly servingDate);
}