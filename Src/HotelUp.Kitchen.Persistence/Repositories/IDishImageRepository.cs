using Microsoft.AspNetCore.Http;

namespace HotelUp.Kitchen.Persistence.Repositories;

public interface IDishImageRepository
{
    Task<string> UploadImageAsync(IFormFile image);
}