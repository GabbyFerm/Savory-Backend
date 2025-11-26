namespace Application.DTOs;

/// <summary>
/// DTO for creating a new recipe
/// </summary>
public class RecipeCreateDto
{
    /// <summary>
    /// Recipe title (required, max 200 characters)
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Recipe description (max 1000 characters)
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
    /// Category ID
    /// </summary>
    public Guid CategoryId { get; set; }

    /// <summary>
    /// List of ingredients with quantities
    /// </summary>
    public List<RecipeIngredientCreateDto> Ingredients { get; set; } = new();
}