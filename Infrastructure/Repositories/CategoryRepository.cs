using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

/// <summary>
/// Repository for Category entity
/// </summary>
public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Gets recipe counts per category for a specific user
    /// </summary>
    public async Task<Dictionary<Guid, int>> GetCategoryRecipeCountsAsync(Guid userId)
    {
        return await _context.Recipes
            .Where(r => r.UserId == userId)
            .GroupBy(r => r.CategoryId)
            .Select(g => new { CategoryId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.CategoryId, x => x.Count);
    }
}