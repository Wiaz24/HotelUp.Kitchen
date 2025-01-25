using HotelUp.Kitchen.Persistence.EFCore;
using HotelUp.Kitchen.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelUp.Kitchen.Persistence.Repositories;

public class CookRepository : ICookRepository
{
    private readonly AppDbContext _dbContext;

    public CookRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Cook?> GetByIdAsync(Guid id)
    {
        return _dbContext.Cooks
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Cook cook)
    {
        await _dbContext.Cooks.AddAsync(cook);
        await _dbContext.SaveChangesAsync();
    }
}