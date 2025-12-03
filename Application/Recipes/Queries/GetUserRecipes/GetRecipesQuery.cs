using Application.Common.Models;
using Application.DTOs;
using MediatR;

namespace Application.Recipes.Queries.GetUserRecipes;

public class GetRecipesQuery : IRequest<OperationResult<PagedResult<RecipeDto>>>
{
    public string? SearchTerm { get; set; }
    public Guid? CategoryId { get; set; }
    public string? SortBy { get; set; }
    public string? SortOrder { get; set; }
    public string? IngredientName { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}