using Application.Common.Models;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Recipes.Commands.UpdateRecipe;

/// <summary>
/// Handler for updating an existing recipe
/// </summary>
public class UpdateRecipeCommandHandler : IRequestHandler<UpdateRecipeCommand, OperationResult<RecipeDto>>
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public UpdateRecipeCommandHandler(IRecipeRepository recipeRepository, IMapper mapper, ICurrentUserService currentUserService)
    {
        _recipeRepository = recipeRepository;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<OperationResult<RecipeDto>> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
    {
        // Get current user ID
        var userId = _currentUserService.GetUserId();
        if (userId == null)
        {
            return OperationResult<RecipeDto>.Failure("User not authenticated");
        }

        // Load existing recipe WITH ingredients
        var recipe = await _recipeRepository.GetRecipeWithDetailsAsync(request.Id);
        if (recipe == null)
        {
            return OperationResult<RecipeDto>.Failure("Recipe not found");
        }

        // Authorization check
        if (recipe.UserId != userId.Value)
        {
            return OperationResult<RecipeDto>.Failure("You can only edit your own recipes");
        }

        // Update recipe properties
        recipe.Title = request.Title;
        recipe.Description = request.Description;
        recipe.Instructions = request.Instructions;
        recipe.PrepTime = request.PrepTime;
        recipe.CookTime = request.CookTime;
        recipe.Servings = request.Servings;
        recipe.CategoryId = request.CategoryId;
        recipe.UpdatedAt = DateTime.UtcNow;

        // Update ingredients - CRITICAL: Remove old ones properly
        recipe.RecipeIngredients.Clear();


        // Add new ingredients
        foreach (var ingredientDto in request.Ingredients)
        {
            recipe.RecipeIngredients.Add(new RecipeIngredient
            {
                RecipeId = recipe.Id,
                IngredientId = ingredientDto.IngredientId,
                Quantity = ingredientDto.Quantity
            });
        }

        // Save to database
        await _recipeRepository.UpdateAsync(recipe);
        await _recipeRepository.SaveChangesAsync();

        // Return success
        return OperationResult<RecipeDto>.Success(null);
    }
}