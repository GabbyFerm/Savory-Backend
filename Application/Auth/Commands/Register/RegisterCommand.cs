using Application.Common.Models;
using Application.DTOs;
using MediatR;

namespace Application.Auth.Commands.Register;

/// <summary>
/// Command to register a new user
/// </summary>
public class RegisterCommand : IRequest<OperationResult<AuthResponseDto>>
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? AvatarColor { get; set; }
}