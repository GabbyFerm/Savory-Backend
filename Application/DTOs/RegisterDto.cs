namespace Application.DTOs;

/// <summary>
/// DTO for user registration
/// </summary>
public class RegisterDto
{
    /// <summary>
    /// Username (required)
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Email adress (required)
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <sumamry>
    /// Password (required, min 6 characters)
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Avatar color hex code (optional, defaults to random)
    /// </summary>
    public string? AvatarColor { get; set; }
}