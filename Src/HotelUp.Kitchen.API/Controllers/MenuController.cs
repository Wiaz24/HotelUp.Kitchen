using System.ComponentModel;
using System.Security.Claims;
using HotelUp.Kitchen.Persistence.DTOs;
using HotelUp.Kitchen.Services.Services;
using HotelUp.Kitchen.Shared.Auth;
using HotelUp.Kitchen.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HotelUp.Kitchen.API.Controllers;

[ApiController]
[Route("api/kitchen/menu")]
[ProducesErrorResponseType(typeof(ErrorResponse))]
public class MenuController : ControllerBase
{
    private readonly IMenuService _menuService;
    private readonly TimeProvider _timeProvider;

    public MenuController(IMenuService menuService, TimeProvider timeProvider)
    {
        _menuService = menuService;
        _timeProvider = timeProvider;
    }

    private Guid LoggedInUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)
        is { } id
        ? new Guid(id)
        : throw new TokenException("No user id found in access token.");
    
    [HttpGet("published")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation("Get published menu by serving date. If not provided, returns today's menu.")]
    public async Task<IActionResult> GetMenu([FromQuery] DateOnly? servingDate)
    {
        var date = servingDate ?? DateOnly.FromDateTime(_timeProvider.GetUtcNow().Date);
        var result = await _menuService.GetPublishedMenuByServingDateAsync(date);
        if (result is null)
        {
            return NotFound();
        }
        return Ok(result);
    }
    
    [HttpGet]
    [Authorize(Policy = PoliciesNames.CanManageDishes)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [SwaggerOperation("Get menus for provided cook. Requires to be in role: Admins/Cooks")]
    public async Task<IActionResult> GetMenusForCook()
    {
        var result = await _menuService.GetMenusByCookIdAsync(LoggedInUserId);
        return Ok(result);
    }
    
    [HttpPost]
    [Authorize(Policy = PoliciesNames.CanManageDishes)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [SwaggerOperation("Create new menu for provided date. Requires to be in role: Admins/Cooks")]
    public async Task<IActionResult> CreateMenu([FromBody] CreateMenuDto dto)
    {
        await _menuService.CreateAsync(LoggedInUserId, dto);
        return CreatedAtAction(nameof(GetMenu), new {servingDate = dto.ServingDate}, null);
    }
    
    [HttpPut("publish")]
    [Authorize(Policy = PoliciesNames.CanManageDishes)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation("Publish menu for provided date. Requires to be menu owner.")]
    public async Task<IActionResult> PublishMenu([FromQuery] PublishMenuDto dto)
    {
        await _menuService.PublishAsync(LoggedInUserId, dto.ServingDate);
        return Ok();
    }
}