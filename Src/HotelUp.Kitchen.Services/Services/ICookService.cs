using HotelUp.Kitchen.Persistence.Entities;

namespace HotelUp.Kitchen.Services.Services;

public interface ICookService
{
    Task<Cook?> GetAsync(Guid id);
    Task CreateAsync(Guid id);
}