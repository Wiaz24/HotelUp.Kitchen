using HotelUp.Kitchen.Persistence.Entities;

namespace HotelUp.Kitchen.Persistence.Repositories;

public interface IMenuRepository
{
    Task<Menu?> GetByServingDateAsync(DateOnly publishDate);
    Task AddAsync(Menu menu);
    Task UpdateAsync(Menu menu);
}