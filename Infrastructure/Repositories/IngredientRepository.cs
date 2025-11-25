using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class IngredientRepository : GenericRepository<Ingredient>, IIngredientRepository
{
    public IngredientRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Ingredient?> GetByNameAsync(string name)
    {
        return await _dbSet
            .FirstOrDefaultAsync(i => i.Name.ToLower() == name.ToLower());
    }
}