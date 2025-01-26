using HotelUp.Kitchen.Persistence.Entities;

namespace HotelUp.Kitchen.Persistence.Repositories;

public interface IDishRepository
{
    Task<Dish?> GetByNameAsync(string name);
    Task<IEnumerable<Dish>> SearchByNameAsync(string searchString);
    Task<IEnumerable<Dish>> GetByNameListAsync(IEnumerable<string> names);
    Task AddAsync(Dish dish);
    Task UpdateAsync(Dish dish);
    Task DeleteAsync(Dish dish);
}