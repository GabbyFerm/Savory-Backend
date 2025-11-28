using Application.Common.Models;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Profile.Commands.UpdateProfile;

/// <summary>
/// Handler for updating user profile
/// </summary>
public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, OperationResult<UserProfileDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public UpdateProfileCommandHandler(UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService, IMapper mapper)
    {
        _userManager = userManager;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<OperationResult<UserProfileDto>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        // Get current user ID
        var userId = _currentUserService.GetUserId();
        if (userId == null)
        {
            return OperationResult<UserProfileDto>.Failure("User not authenticated");
        }

        // Get user from database
        var user = await _userManager.FindByIdAsync(userId.Value.ToString());
        if (user == null)
        {
            return OperationResult<UserProfileDto>.Failure("User not found");
        }

        // Update username if provided
        if (!string.IsNullOrWhiteSpace(request.UserName) && request.UserName != user.UserName)
        {
            user.UserName = request.UserName;
        }

        // Update email if provided
        if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != user.Email)
        {
            // Check if email is already taken
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null && existingUser.Id != user.Id)
            {
                return OperationResult<UserProfileDto>.Failure("Email is already taken");
            }

            user.Email = request.Email;
        }

        // Update avatar color if provided
        if (!string.IsNullOrWhiteSpace(request.AvatarColor))
        {
            user.AvatarColor = request.AvatarColor;
        }

        // Save changes
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return OperationResult<UserProfileDto>.Failure(
                string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        // Return updated profile
        var userDto = _mapper.Map<UserProfileDto>(user);
        return OperationResult<UserProfileDto>.Success(userDto);
    }
}