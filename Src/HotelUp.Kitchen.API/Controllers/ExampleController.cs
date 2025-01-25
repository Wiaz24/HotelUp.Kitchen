using System.Security.Claims;
using HotelUp.Kitchen.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HotelUp.Kitchen.API.Controllers;

[ApiController]
[Route("api/kitchen/example")]
[ProducesErrorResponseType(typeof(ErrorResponse))]
public class ExampleController : ControllerBase
{
    private Guid LoggedInUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)
        is { } id
        ? new Guid(id)
        : throw new TokenException("No user id found in access token.");


    [Authorize]
    [HttpGet("logged-in-user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Returns information about the logged in user")]
    public IActionResult GetLoggedInUser()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        return Ok($"Hello {email}!");
    }
}