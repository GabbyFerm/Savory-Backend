using Application.Common.Models;
using MediatR;

namespace Application.Profile.Commands.ChangePassword;

/// <summary>
/// Command to change user password
/// </summary>
public class ChangePasswordCommand : IRequest<OperationResult>
{
    /// <summary>
    /// Current password
    /// </summary>
    public string CurrentPassword { get; set; } = string.Empty;

    /// <summary>
    /// New password
    /// </summary>
    public string NewPassword { get; set; } = string.Empty;

    /// <summary>
    /// Confirm new password
    /// </summary>
    public string ConfirmPassword { get; set; } = string.Empty;
}