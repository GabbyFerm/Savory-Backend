using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RecipeRepository : GenericRepository<Recipe>, IRecipeRepository
{
    public RecipeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Recipe>> GetUserRecipesAsync(Guid userId, string? searchTerm, Guid? categoryId, string? sortBy, string? sortOrder, string? ingredientName)
    {
        // All recipes belonging to user
        IQueryable<Recipe> query = _dbSet
            .Where(r => r.UserId == userId)
            .Include(r => r.Category);

        // Filter by category
        if (categoryId.HasValue && categoryId != Guid.Empty)
        {
            query = query.Where(r => r.CategoryId == categoryId.Value);
        }

        // Search by Title
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(r => r.Title.Contains(searchTerm));
        }

        // Search by Ingredient name
        if (!string.IsNullOrWhiteSpace(ingredientName))
        {
            query = query.Where(r => r.RecipeIngredients.Any(ri =>
                ri.Ingredient.Name.ToLower().Contains(ingredientName.ToLower())));
        }

        // Sort by title, createdDate, cookTime
        query = ApplySorting(query, sortBy, sortOrder);

        return await query.ToListAsync();
    }

    public async Task<Recipe?> GetRecipeWithDetailsAsync(Guid recipeId)
    {
        return await _dbSet
            .Include(r => r.Category)
            .Include(r => r.RecipeIngredients)
            .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefaultAsync(r => r.Id == recipeId);
    }

    private IQueryable<Recipe> ApplySorting(IQueryable<Recipe> query, string? sortBy, string? sortOrder)
    {
        // Simple switch to handle sorting logic
        var isDescending = sortOrder?.Equals("desc", StringComparison.OrdinalIgnoreCase) ?? false;

        switch (sortBy?.ToLower())
        {
            case "title":
                query = isDescending ? query.OrderByDescending(r => r.Title) : query.OrderBy(r => r.Title);
                break;
            case "cooktime":
                query = isDescending ? query.OrderByDescending(r => r.CookTime) : query.OrderBy(r => r.CookTime);
                break;
            case "createddate":
                query = isDescending ? query.OrderByDescending(r => r.CreatedAt) : query.OrderBy(r => r.CreatedAt);
                break;

            default:
                query = query.OrderByDescending(r => r.CreatedAt);
                break;
        }

        return query;
    }
}