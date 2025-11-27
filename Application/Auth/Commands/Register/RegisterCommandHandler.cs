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
        // Create user with Identity
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
            return OperationResult<AuthResponseDto>.Failure(
                string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        // Generate JWT token
        var token = _jwtService.GenerateToken(newUser);

        // Return response
        return OperationResult<AuthResponseDto>.Success(new AuthResponseDto
        {
            Token = token,
            User = _mapper.Map<UserProfileDto>(newUser)
        });
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