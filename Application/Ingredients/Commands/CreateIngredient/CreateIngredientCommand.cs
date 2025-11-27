using Application.Common.Models;
using Application.DTOs;
using MediatR;

namespace Application.Ingredients.Commands.CreateIngredient;

/// <summary>
/// Command to create a new ingredient
/// </summary>
public class CreateIngredientCommand : IRequest<OperationResult<IngredientDto>>
{
    /// <summary>
    /// Ingredient name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Unit of measurement (g, ml, pcs, etc.)
    /// </summary>
    public string Unit { get; set; } = string.Empty;
}