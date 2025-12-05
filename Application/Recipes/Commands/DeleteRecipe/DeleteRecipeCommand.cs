using Application.Common.Models;
using MediatR;

namespace Application.Recipes.Commands.DeleteRecipe;

/// <summary>
/// Command to delete a recipe
/// </summary>
public class DeleteRecipeCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
}