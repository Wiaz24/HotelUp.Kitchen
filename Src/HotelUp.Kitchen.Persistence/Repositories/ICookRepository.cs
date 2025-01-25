using HotelUp.Kitchen.Persistence.Entities;

namespace HotelUp.Kitchen.Persistence.Repositories;

public interface ICookRepository
{
    Task<Cook?> GetByIdAsync(Guid id);
    Task AddAsync(Cook cook);    
}