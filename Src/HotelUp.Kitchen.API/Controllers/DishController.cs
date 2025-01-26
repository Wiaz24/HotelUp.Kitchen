using System.Security.Claims;
using HotelUp.Kitchen.Persistence.Entities;
using HotelUp.Kitchen.Services.DTOs;
using HotelUp.Kitchen.Services.Services;
using HotelUp.Kitchen.Shared.Auth;
using HotelUp.Kitchen.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HotelUp.Kitchen.API.Controllers;

[ApiController]
[Route("api/kitchen/dish")]
[ProducesErrorResponseType(typeof(ErrorResponse))]
public class DishController : ControllerBase
{
    private readonly IDishService _dishService;

    public DishController(IDishService dishService)
    {
        _dishService = dishService;
    }

    private Guid LoggedInUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)
        is { } id
        ? new Guid(id)
        : throw new TokenException("No user id found in access token.");

    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation("Search for dishes by name")]
    public async Task<ActionResult<IEnumerable<Dish>>> SearchForDishesByName([FromQuery] SearchForDishesDto searchDto)
    {
        var result = await _dishService.SearchDishByNameAsync(searchDto.SearchString);
        return Ok(result);
    }
    
    [HttpPost]
    [Authorize(Policy = PoliciesNames.CanManageDishes)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [SwaggerOperation("Create a new dish. Requires to be admin or cook.")]
    public async Task<ActionResult> CreateDish([FromForm] CreateDishDto dishDto)
    {
        var decimalPrice = (decimal)dishDto.Price;
        await _dishService.CreateDishAsync(dishDto.Name, decimalPrice, dishDto.Image);
        return CreatedAtAction(nameof(SearchForDishesByName), new { searchString = dishDto.Name }, null);
    }
}