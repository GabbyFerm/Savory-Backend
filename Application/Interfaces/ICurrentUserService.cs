namespace Application.Interfaces;

/// <summary>
/// Service for getting current user information from HTTP context
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets the ID of the currently authenticated user
    /// </summary>
    Guid? GetUserId();
}