namespace Application.DTOs;

/// <summary>
/// Category data transfer object with recipe count
/// </summary>
public class CategoryWithCountDto
{
    /// <summary>
    /// Unique category identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Category name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Number of recipes in this category
    /// </summary>
    public int RecipeCount { get; set; }
}