using HotelUp.Kitchen.Persistence.EFCore;
using HotelUp.Kitchen.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelUp.Kitchen.Persistence.Repositories;

public class DishRepository : IDishRepository
{
    private readonly AppDbContext _dbContext;

    public DishRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Dish?> GetByNameAsync(string name)
    {
        return _dbContext.Dishes
            .FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<IEnumerable<Dish>> SearchByNameAsync(string searchString)
    {
        return await _dbContext.Dishes
            .Where(x => EF.Functions.ILike(x.Name, $"%{searchString}%"))
            .ToListAsync();
    }

    public async Task AddAsync(Dish dish)
    {
        await _dbContext.Dishes.AddAsync(dish);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Dish dish)
    {
        _dbContext.Dishes.Update(dish);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Dish dish)
    {
        _dbContext.Dishes.Remove(dish);
        await _dbContext.SaveChangesAsync();
    }
}