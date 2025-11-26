namespace Application.DTOs;

/// <summary>
/// Ingredient data transfer object
/// </summary>
public class IngredientDto
{
    /// <summary>
    /// Unique ingredient identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Ingredient name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Unit of measurement (g, ml, pcs, etc.)
    /// </summary>
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// When the ingredient was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
}