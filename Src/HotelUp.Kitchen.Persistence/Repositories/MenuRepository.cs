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

    public Task<Menu?> GetPublishedMenuByServingDateAsync(DateOnly publishDate)
    {
        return _dbContext.Menus
            .AsNoTracking()
            .Include(x => x.Dishes)
            .Where(x => x.Published == true)
            .FirstOrDefaultAsync(x => x.ServingDate == publishDate);
    }

    public Task<Menu?> GetByDateAsync(DateOnly servingDate)
    {
        return _dbContext.Menus
            .Include(x => x.Cook)
            .Include(x => x.Dishes)
            .FirstOrDefaultAsync(x => x.ServingDate == servingDate);
    }

    public async Task<IEnumerable<Menu>> GetByCookIdAsync(Guid cookId)
    {
        return await _dbContext.Menus
            .Include(x => x.Cook)
            .Include(x => x.Dishes)
            .Where(x => x.Cook.Id == cookId)
            .ToListAsync();
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