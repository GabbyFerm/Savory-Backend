using Application.Common.Models;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, OperationResult<AuthResponseDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;

    public RefreshTokenCommandHandler(
        UserManager<ApplicationUser> userManager,
        IJwtService jwtService,
        IMapper mapper)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _mapper = mapper;
    }

    public async Task<OperationResult<AuthResponseDto>> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        // Validate access token (without checking expiry)
        var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal == null)
        {
            return OperationResult<AuthResponseDto>.Failure("Invalid access token");
        }

        var userId = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return OperationResult<AuthResponseDto>.Failure("Invalid token claims");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return OperationResult<AuthResponseDto>.Failure("User not found");
        }

        // Validate refresh token
        if (user.RefreshToken != request.RefreshToken)
        {
            return OperationResult<AuthResponseDto>.Failure("Invalid refresh token");
        }

        if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return OperationResult<AuthResponseDto>.Failure("Refresh token has expired");
        }

        // Generate new tokens
        var newAccessToken = _jwtService.GenerateToken(user);
        var newRefreshToken = _jwtService.GenerateRefreshToken();

        // Update refresh token in database
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        var userDto = _mapper.Map<UserProfileDto>(user);

        var authResponse = new AuthResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            User = userDto
        };

        return OperationResult<AuthResponseDto>.Success(authResponse);
    }
}