namespace Application.DTOs;

/// <summary>
/// DTO for adding ingredients to a recipe
/// </summary>
public class RecipeIngredientCreateDto
{
    /// <summary>
    /// Ingredient ID (must exist in database)
    /// </summary>
    public Guid IngredientId { get; set; }

    /// <summary>
    /// Quantity of ingredient
    /// </summary>
    public decimal Quantity { get; set; }
}