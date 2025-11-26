using Domain.Entities;

namespace Application.Interfaces;

public interface IRecipeRepository : IGenericRepository<Recipe>
{
    Task<IEnumerable<Recipe>> GetUserRecipesAsync(Guid userId, string? searchTerm, Guid? categoryId, string? sortBy,
        string? sortOrder, string? ingredientName);
    Task<Recipe?> GetRecipeWithDetailsAsync(Guid recipeId);

    // Dashboard statistics methods

    /// Gets the total number of recipes for a user
    Task<int> GetUserRecipeCountAsync(Guid userId);

    /// Gets the most recent recipes for a user
    Task<IEnumerable<Recipe>> GetRecentRecipesAsync(Guid userId, int count = 5);

    /// Gets recipe count grouped by category for a user
    Task<Dictionary<Guid, int>> GetRecipeCountByCategoryAsync(Guid userId);

    /// Gets the average cook time for all user's recipes
    Task<double> GetAverageCookTimeAsync(Guid userId);

    /// Gets the average total time (prep + cook) for all user's recipes
    Task<double> GetAverageTotalTimeAsync(Guid userId);
}