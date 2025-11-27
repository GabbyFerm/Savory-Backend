using Application.Common.Models;
using Application.DTOs;
using MediatR;

namespace Application.Recipes.Queries.GetUserRecipes;

public class GetRecipesQuery : IRequest<OperationResult<IEnumerable<RecipeDto>>>
{
    public string? SearchTerm { get; set; }
    public Guid? CategoryId { get; set; }
    public string? SortBy { get; set; }
    public string? SortOrder { get; set; }
    public string? IngredientName { get; set; }
}