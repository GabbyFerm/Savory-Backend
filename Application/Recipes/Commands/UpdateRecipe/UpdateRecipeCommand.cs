using Application.Common.Models;
using Application.DTOs;
using MediatR;

namespace Application.Recipes.Commands.UpdateRecipe;

public class UpdateRecipeCommand : IRequest<OperationResult<RecipeDto>>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Instructions { get; set; } = string.Empty;
    public int PrepTime { get; set; }
    public int CookTime { get; set; }
    public int Servings { get; set; }
    public Guid CategoryId { get; set; }
    public List<RecipeIngredientCreateDto> Ingredients { get; set; } = new();
}