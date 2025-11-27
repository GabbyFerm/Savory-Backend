using Application.Common.Models;
using Application.DTOs;
using MediatR;

namespace Application.Ingredients.Queries.GetIngredientById;

/// <summary>
/// Query to get a single ingredient by ID
/// </summary>
public class GetIngredientByIdQuery : IRequest<OperationResult<IngredientDto>>
{
    /// <summary>
    /// Ingredient ID
    /// </summary>
    public Guid Id { get; set; }
}