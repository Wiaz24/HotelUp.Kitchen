using System.Security.Claims;
using HotelUp.Kitchen.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HotelUp.Kitchen.API.Controllers;

[ApiController]
[Route("api/kitchen/menu")]
[ProducesErrorResponseType(typeof(ErrorResponse))]
public class MenuRepository : ControllerBase
{
    private Guid LoggedInUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)
        is { } id
        ? new Guid(id)
        : throw new TokenException("No user id found in access token.");

    public MenuRepository()
    {
        
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation("Get menu by serving date. If not provided, returns today's menu.")]
    public async Task<IActionResult> GetMenu([FromQuery] DateOnly? servingDate)
    {
        
        return Ok();
    }
    
        
}