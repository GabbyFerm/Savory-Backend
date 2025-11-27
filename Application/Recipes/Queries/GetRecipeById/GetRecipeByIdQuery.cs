using Application.Common.Models;
using Application.DTOs;
using MediatR;

namespace Application.Recipes.Queries.GetRecipeById;

/// <summary>
/// Query to get a single recipe by ID
/// </summary>
public class GetRecipeByIdQuery : IRequest<OperationResult<RecipeDto>>
{
    public Guid Id { get; set; }
}