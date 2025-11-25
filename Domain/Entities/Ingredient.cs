using Domain.Common;

namespace Domain.Entities;

public class Ingredient : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;  // "g", "ml", "pcs"

    // Navigation
    public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}