using Application.Auth.Commands.Login;
using Application.Auth.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand? command)
    {
        if (command == null)
        {
            return BadRequest(new { Message = "Command is null - deserialization failed" });
        }

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(new { Message = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand? command)
    {
        if (command == null)
        {
            return BadRequest(new { Message = "Command is null - deserialization failed" });
        }

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(new { Message = result.ErrorMessage });
        }

        return Ok(result.Data);
    }
}
