using Application.Profile.Commands.ChangePassword;
using Application.Profile.Commands.UpdateProfile;
using Application.Profile.Queries.GetProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// User profile management endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class ProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets current user's profile
    /// </summary>
    /// <returns>User profile information</returns>
    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var query = new GetProfileQuery();
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            return BadRequest(new { Message = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Updates current user's profile
    /// </summary>
    /// <param name="command">Profile update data</param>
    /// <returns>Updated user profile</returns>
    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command)
    {
        if (command == null)
        {
            return BadRequest(new { Message = "Invalid request body" });
        }

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(new { Message = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Changes current user's password
    /// </summary>
    /// <param name="command">Password change data</param>
    /// <returns>Success or error message</returns>
    [HttpPut("password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
    {
        if (command == null)
        {
            return BadRequest(new { Message = "Invalid request body" });
        }

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(new { Message = result.ErrorMessage });
        }

        return Ok(new { Message = "Password changed successfully" });
    }
}
