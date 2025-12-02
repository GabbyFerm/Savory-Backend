using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

/// <summary>
/// Repository for Recipe entity with specialized query methods
/// </summary>
public class RecipeRepository : GenericRepository<Recipe>, IRecipeRepository
{
    public RecipeRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Gets all recipes for a specific user with optional filtering and sorting
    /// </summary>
    public async Task<IEnumerable<Recipe>> GetUserRecipesAsync(Guid userId, string? searchTerm, Guid? categoryId, string? sortBy, string? sortOrder, string? ingredientName)
    {
        // Start with user's recipes and include related data
        IQueryable<Recipe> query = _dbSet
            .Where(r => r.UserId == userId)
            .Include(r => r.Category)
            .Include(r => r.RecipeIngredients)
            .ThenInclude(ri => ri.Ingredient);

        // Filter by category
        if (categoryId.HasValue && categoryId != Guid.Empty)
        {
            query = query.Where(r => r.CategoryId == categoryId.Value);
        }

        // Combined search with OR logic when both are the same
        // (Frontend unified search sends same value to both)
        if (!string.IsNullOrWhiteSpace(searchTerm) && !string.IsNullOrWhiteSpace(ingredientName))
        {
            var lowerSearchTerm = searchTerm.ToLower();
            var lowerIngredientName = ingredientName.ToLower();

            // If they're the same value, use OR logic (unified search)
            if (lowerSearchTerm == lowerIngredientName)
            {
                query = query.Where(r =>
                    r.Title.ToLower().Contains(lowerSearchTerm) ||
                    r.RecipeIngredients.Any(ri => ri.Ingredient.Name.ToLower().Contains(lowerIngredientName))
                );
            }
            else
            {
                // If different values, use AND logic (advanced search)
                query = query.Where(r =>
                    r.Title.ToLower().Contains(lowerSearchTerm) &&
                    r.RecipeIngredients.Any(ri => ri.Ingredient.Name.ToLower().Contains(lowerIngredientName))
                );
            }
        }
        // Single parameter provided - apply that filter
        else if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var lowerSearchTerm = searchTerm.ToLower();
            query = query.Where(r => r.Title.ToLower().Contains(lowerSearchTerm));
        }
        else if (!string.IsNullOrWhiteSpace(ingredientName))
        {
            var lowerIngredientName = ingredientName.ToLower();
            query = query.Where(r => r.RecipeIngredients.Any(ri =>
                ri.Ingredient.Name.ToLower().Contains(lowerIngredientName)));
        }

        // Apply sorting
        query = ApplySorting(query, sortBy, sortOrder);

        return await query.ToListAsync();
    }

    /// <summary>
    /// Gets a single recipe with all related data (category and ingredients)
    /// </summary>
    public async Task<Recipe?> GetRecipeWithDetailsAsync(Guid recipeId)
    {
        return await _dbSet
            .Include(r => r.Category)
            .Include(r => r.RecipeIngredients)
            .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefaultAsync(r => r.Id == recipeId);
    }

    /// <summary>
    /// Gets the total number of recipes for a user
    /// </summary>
    public async Task<int> GetUserRecipeCountAsync(Guid userId)
    {
        return await _dbSet
            .Where(r => r.UserId == userId)
            .CountAsync();
    }

    /// <summary>
    /// Gets the most recent recipes for a user
    /// </summary>
    public async Task<IEnumerable<Recipe>> GetRecentRecipesAsync(Guid userId, int count = 5)
    {
        return await _dbSet
            .Where(r => r.UserId == userId)
            .Include(r => r.Category)
            .OrderByDescending(r => r.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    /// <summary>
    /// Gets recipe count grouped by category for a user
    /// </summary>
    public async Task<Dictionary<Guid, int>> GetRecipeCountByCategoryAsync(Guid userId)
    {
        return await _dbSet
            .Where(r => r.UserId == userId)
            .GroupBy(r => r.CategoryId)
            .Select(g => new { CategoryId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.CategoryId, x => x.Count);
    }

    /// <summary>
    /// Gets the average cook time for all user's recipes
    /// </summary>
    public async Task<double> GetAverageCookTimeAsync(Guid userId)
    {
        var recipes = await _dbSet
            .Where(r => r.UserId == userId)
            .ToListAsync();

        // Return 0 if no recipes
        if (!recipes.Any())
            return 0;

        return recipes.Average(r => r.CookTime);
    }

    /// <summary>
    /// Gets the average total time (prep + cook) for all user's recipes
    /// </summary>
    public async Task<double> GetAverageTotalTimeAsync(Guid userId)
    {
        var recipes = await _dbSet
            .Where(r => r.UserId == userId)
            .ToListAsync();

        // Return 0 if no recipes
        if (!recipes.Any())
            return 0;

        return recipes.Average(r => r.PrepTime + r.CookTime);
    }

    /// <summary>
    /// Applies sorting to the recipe query
    /// </summary>
    private IQueryable<Recipe> ApplySorting(IQueryable<Recipe> query, string? sortBy, string? sortOrder)
    {
        var isDescending = sortOrder?.Equals("desc", StringComparison.OrdinalIgnoreCase) ?? false;

        switch (sortBy?.ToLower())
        {
            case "title":
                query = isDescending
                    ? query.OrderByDescending(r => r.Title)
                    : query.OrderBy(r => r.Title);
                break;

            case "cooktime":
                query = isDescending
                    ? query.OrderByDescending(r => r.CookTime)
                    : query.OrderBy(r => r.CookTime);
                break;

            case "createddate":
                query = isDescending
                    ? query.OrderByDescending(r => r.CreatedAt)
                    : query.OrderBy(r => r.CreatedAt);
                break;

            default:
                // Default: newest first
                query = query.OrderByDescending(r => r.CreatedAt);
                break;
        }

        return query;
    }
}