using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? AvatarColor { get; set; }  // Hex color for avatar

    // Navigation
    public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}