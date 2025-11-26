namespace Application.DTOs;

/// <summary>
/// Recipe ingredient with quantity
/// </summary>
public class RecipeIngredientDto
{
    /// <summary>
    /// Ingredient ID
    /// </summary>
    public Guid IngredientId { get; set; }

    /// <summary>
    /// Ingredient name
    /// </summary>
    public string IngredientName { get; set; } = string.Empty;

    /// <summary>
    /// Quantity of ingredient
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// Unit of measurement (g, ml, pcs, etc.)
    /// </summary>
    public string Unit { get; set; } = string.Empty;
}