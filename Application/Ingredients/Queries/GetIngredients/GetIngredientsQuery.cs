using Application.Common.Models;
using Application.DTOs;
using MediatR;

namespace Application.Ingredients.Queries.GetIngredients;

/// <summary>
/// Query to get all ingredients with optional search
/// </summary>
public class GetIngredientsQuery : IRequest<OperationResult<List<IngredientDto>>>
{
    /// <summary>
    /// Optional search term to filter ingredients by name
    /// </summary>
    public string? SearchTerm { get; set; }
}