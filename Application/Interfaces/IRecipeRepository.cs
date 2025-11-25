using Domain.Entities;

namespace Application.Interfaces;

public interface IRecipeRepository : IGenericRepository<Recipe>
{
    Task<IEnumerable<Recipe>> GetUserRecipesAsync(Guid userId, string? searchTerm, Guid? categoryId, string? sortBy, string? sortOrder, string? ingredientName);

    Task<Recipe?> GetRecipeWithDetailsAsync(Guid recipeId);
}