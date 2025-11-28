using Application.Common.Models;
using Application.DTOs;
using MediatR;

namespace Application.Profile.Commands.UpdateProfile;

/// <summary>
/// Command to update user profile
/// </summary>
public class UpdateProfileCommand : IRequest<OperationResult<UserProfileDto>>
{
    /// <summary>
    /// New username (optional)
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// New email address (optional)
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// New avatar color hex code (optional)
    /// </summary>
    public string? AvatarColor { get; set; }
}