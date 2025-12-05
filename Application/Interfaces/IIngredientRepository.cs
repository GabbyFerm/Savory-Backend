using Domain.Entities;

namespace Application.Interfaces;

public interface IIngredientRepository : IGenericRepository<Ingredient>
{
    Task<Ingredient?> GetByNameAsync(string name);
}