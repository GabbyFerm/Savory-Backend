using Application.Common.Models;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Recipes.Commands.DeleteRecipe;

/// <summary>
/// Handler for deleting a recipe
/// </summary>
public class DeleteRecipeCommandHandler : IRequestHandler<DeleteRecipeCommand, OperationResult>
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteRecipeCommandHandler(IRecipeRepository recipeRepository, ICurrentUserService currentUserService)
    {
        _recipeRepository = recipeRepository;
        _currentUserService = currentUserService;
    }

    public async Task<OperationResult> Handle(DeleteRecipeCommand request, CancellationToken cancellationToken)
    {
        // Get current user ID
        var userId = _currentUserService.GetUserId();
        if (userId == null)
        {
            return OperationResult.Failure("User not authenticated");
        }

        // Load existing recipe
        var recipe = await _recipeRepository.GetByIdAsync(request.Id);
        if (recipe == null)
        {
            return OperationResult.Failure("Recipe not found");
        }

        // Authorization check
        if (recipe.UserId != userId.Value)
        {
            return OperationResult.Failure("You can only delete your own recipes");
        }

        await _recipeRepository.DeleteAsync(request.Id);
        await _recipeRepository.SaveChangesAsync();

        return OperationResult.Success();
    }
}