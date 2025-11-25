using Domain.Common;

namespace Domain.Entities;

public class Recipe : BaseEntity
{
    public Guid UserId { get; set; }  // Owner

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Instructions { get; set; } = string.Empty;
    public int PrepTime { get; set; }
    public int CookTime { get; set; }
    public int Servings { get; set; }
    public string? ImagePath { get; set; }
    public Guid CategoryId { get; set; }

    // Navigation
    public ApplicationUser User { get; set; } = null!;
    public Category Category { get; set; } = null!;
    public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}