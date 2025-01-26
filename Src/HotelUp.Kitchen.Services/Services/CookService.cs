using HotelUp.Kitchen.Persistence.Entities;
using HotelUp.Kitchen.Persistence.Repositories;
using HotelUp.Kitchen.Services.Services.Exceptions;

namespace HotelUp.Kitchen.Services.Services;

public class CookService : ICookService
{
    private readonly ICookRepository _cookRepository;

    public CookService(ICookRepository cookRepository)
    {
        _cookRepository = cookRepository;
    }

    public Task<Cook?> GetAsync(Guid id)
    {
        return _cookRepository.GetByIdAsync(id);
    }

    public async Task CreateAsync(Guid id)
    {
        var existingCook = await _cookRepository.GetByIdAsync(id);
        if (existingCook is not null)
        {
            throw new CookAlreadyExistsException(id);
        }
        var cook = new Cook
        {
            Id = id,
            FoodTasks = []
        };
        await _cookRepository.AddAsync(cook);
    }
}