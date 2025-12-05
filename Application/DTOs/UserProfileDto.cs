namespace Application.DTOs;

/// <summary>
/// User profile data transfer object
/// </summary>
public class UserProfileDto
{
    /// <summary>
    /// User ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Username
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Avatar color (hex code)
    /// </summary>
    public string? AvatarColor { get; set; }
}