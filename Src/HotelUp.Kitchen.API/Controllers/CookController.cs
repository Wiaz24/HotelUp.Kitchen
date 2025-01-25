using System.Security.Claims;
using HotelUp.Kitchen.Services.Services;
using HotelUp.Kitchen.Shared.Auth;
using HotelUp.Kitchen.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HotelUp.Kitchen.API.Controllers;

[ApiController]
[Route("api/kitchen/cook")]
[ProducesErrorResponseType(typeof(ErrorResponse))]
public class CookController : ControllerBase
{
    private readonly ICookService _cookService;

    public CookController(ICookService cookService)
    {
        _cookService = cookService;
    }

    private Guid LoggedInUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)
        is { } id
        ? new Guid(id)
        : throw new TokenException("No user id found in access token.");
    
    [HttpPost]
    [Authorize(Policy = PoliciesNames.CanManageDishes)]
    [Obsolete]
    [SwaggerOperation("Create new cook for logged in user. ONLY FOR TESTING PURPOSES")]
    public async Task<IActionResult> CreateCook()
    {
        await _cookService.CreateAsync(LoggedInUserId);
        return Ok();
    }
    
}