namespace Application.DTOs;

/// <summary>
/// Recipe data transfer object with full details
/// </summary>
public class RecipeDto
{
    /// <summary>
    /// Unique recipe identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Recipe title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Recipe description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Cooking instructions
    /// </summary>
    public string Instructions { get; set; } = string.Empty;

    /// <summary>
    /// Preparation time in minutes
    /// </summary>
    public int PrepTime { get; set; }

    /// <summary>
    /// Cooking time in minutes
    /// </summary>
    public int CookTime { get; set; }

    /// <summary>
    /// Number of servings
    /// </summary>
    public int Servings { get; set; }

    /// <summary>
    /// Path to recipe image (if uploaded)
    /// </summary>
    public string? ImagePath { get; set; }

    /// <summary>
    /// Category ID
    /// </summary>
    public Guid CategoryId { get; set; }

    /// <summary>
    /// Category name
    /// </summary>
    public string CategoryName { get; set; } = string.Empty;

    /// <summary>
    /// List of ingredients with quantities
    /// </summary>
    public List<RecipeIngredientDto> Ingredients { get; set; } = new();

    /// <summary>
    /// When the recipe was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// When the recipe was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}