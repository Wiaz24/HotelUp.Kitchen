using HotelUp.Kitchen.Persistence.EFCore;
using HotelUp.Kitchen.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelUp.Kitchen.Persistence.Repositories;

public class MenuRepository : IMenuRepository
{
    private readonly AppDbContext _dbContext;

    public MenuRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Menu?> GetByServingDateAsync(DateOnly publishDate)
    {
        return _dbContext.Menus
            .Include(x => x.Dishes)
            .FirstOrDefaultAsync(x => x.ServingDate == publishDate);
    }

    public async Task AddAsync(Menu menu)
    {
        await _dbContext.Menus.AddAsync(menu);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Menu menu)
    {
        _dbContext.Menus.Update(menu);
        await _dbContext.SaveChangesAsync();
    }
}