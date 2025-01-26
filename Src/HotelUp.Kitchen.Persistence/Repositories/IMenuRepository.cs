using HotelUp.Kitchen.Persistence.Entities;

namespace HotelUp.Kitchen.Persistence.Repositories;

public interface IMenuRepository
{
    Task<Menu?> GetPublishedMenuByServingDateAsync(DateOnly servingDate);
    Task<Menu?> GetByDateAsync(DateOnly servingDate);
    Task<IEnumerable<Menu>> GetByCookIdAsync(Guid cookId);
    Task AddAsync(Menu menu);
    Task UpdateAsync(Menu menu);
}