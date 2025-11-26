namespace Application.DTOs;

/// <summary>
/// Authentication response with JWT token
/// </summary>
public class AuthResponseDto
{
    /// <summary>
    /// JWT authentication token
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// User profile information
    /// </summary>
    public UserProfileDto User { get; set; } = null!;
}