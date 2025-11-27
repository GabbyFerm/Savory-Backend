using Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Repository interface for Category entity
/// </summary>
public interface ICategoryRepository : IGenericRepository<Category>
{
    /// <summary>
    /// Gets all categories with recipe count for a specific user
    /// </summary>
    /// <param name="userId">User ID to count recipes for</param>
    /// <returns>Dictionary with CategoryId as key and RecipeCount as value</returns>
    Task<Dictionary<Guid, int>> GetCategoryRecipeCountsAsync(Guid userId);
}