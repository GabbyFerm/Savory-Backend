namespace Application.DTOs;

/// <summary>
/// DTO for updating user profile
/// </summary>
public class UpdateProfileDto
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