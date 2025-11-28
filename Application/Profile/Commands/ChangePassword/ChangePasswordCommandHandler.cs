using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Profile.Commands.ChangePassword;

/// <summary>
/// Handler for changing user password
/// </summary>
public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, OperationResult>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICurrentUserService _currentUserService;

    public ChangePasswordCommandHandler(
        UserManager<ApplicationUser> userManager,
        ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _currentUserService = currentUserService;
    }

    public async Task<OperationResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        // Get current user ID
        var userId = _currentUserService.GetUserId();
        if (userId == null)
        {
            return OperationResult.Failure("User not authenticated");
        }

        // Get user from database
        var user = await _userManager.FindByIdAsync(userId.Value.ToString());
        if (user == null)
        {
            return OperationResult.Failure("User no found");
        }

        // Change password
        var result = await _userManager.ChangePasswordAsync(
            user,
            request.CurrentPassword,
            request.NewPassword);

        if (!result.Succeeded)
        {
            return OperationResult.Failure(
                string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return OperationResult.Success();
    }
}