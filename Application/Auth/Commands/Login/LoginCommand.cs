using Application.Common.Models;
using Application.DTOs;
using MediatR;

namespace Application.Auth.Commands.Login;

/// <summary>
/// Command to login a registered user
/// </summary>
public class LoginCommand : IRequest<OperationResult<AuthResponseDto>>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}