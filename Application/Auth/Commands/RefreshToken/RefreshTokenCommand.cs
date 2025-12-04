using Application.Common.Models;
using Application.DTOs;
using MediatR;

namespace Application.Auth.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<OperationResult<AuthResponseDto>>
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}