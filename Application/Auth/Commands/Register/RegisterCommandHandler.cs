using Application.Common.Models;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, OperationResult<AuthResponseDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;

    public RegisterCommandHandler(UserManager<ApplicationUser> userManager, IJwtService jwtService, IMapper mapper)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _mapper = mapper;

    }
    public async Task<OperationResult<AuthResponseDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // Check if user already exists
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return OperationResult<AuthResponseDto>.Failure("User with this email already exists");
        }

        // Create new user
        var newUser = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = request.UserName,
            Email = request.Email,
            AvatarColor = request.AvatarColor ?? GenerateRandomColor()
        };

        var result = await _userManager.CreateAsync(newUser, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return OperationResult<AuthResponseDto>.Failure($"Registration failed: {errors}");
        }

        // Generate tokens
        var accessToken = _jwtService.GenerateToken(newUser);
        var refreshToken = _jwtService.GenerateRefreshToken();

        // Save refresh token
        newUser.RefreshToken = refreshToken;
        newUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(newUser);

        var userDto = _mapper.Map<UserProfileDto>(newUser);

        var authResponse = new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            User = userDto
        };

        return OperationResult<AuthResponseDto>.Success(authResponse);
    }

    private static string GenerateRandomColor()
    {
        var colors = new[]
        {
            "#FF6B6B", "#4ECDC4", "#45B7D1", "#FFA07A",
            "#98D8C8", "#F7DC6F", "#BB8FCE", "#85C1E2",
            "#F8B739", "#52B788", "#E63946", "#457B9D"
        };

        var random = new Random();
        return colors[random.Next(colors.Length)];
    }
}