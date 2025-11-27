using Application.Common.Models;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Recipes.Commands.CreateRecipe;

/// <summary>
/// Handler for creating a new recipe
/// </summary>
public class CreateRecipeCommandHandler : IRequestHandler<CreateRecipeCommand, OperationResult<RecipeDto>>
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CreateRecipeCommandHandler(IRecipeRepository recipeRepository, IMapper mapper, ICurrentUserService currentUserService)
    {
        _recipeRepository = recipeRepository;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<OperationResult<RecipeDto>> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
    {
        // Get current user ID
        var userId = _currentUserService.GetUserId();
        if (userId == null)
        {
            return OperationResult<RecipeDto>.Failure("User not authenticated");
        }

        // Create recipe entity
        var recipe = new Recipe
        {
            Id = Guid.NewGuid(),
            UserId = userId.Value,
            Title = request.Title,
            Description = request.Description,
            Instructions = request.Instructions,
            PrepTime = request.PrepTime,
            CookTime = request.CookTime,
            Servings = request.Servings,
            CategoryId = request.CategoryId,
            CreatedAt = DateTime.UtcNow
        };

        // Add ingredients to recipe
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
        await _recipeRepository.AddAsync(recipe);
        await _recipeRepository.SaveChangesAsync();

        // Load recipe with full details (category and ingredients)
        var createdRecipe = await _recipeRepository.GetRecipeWithDetailsAsync(recipe.Id);

        // Map to DTO
        var recipeDto = _mapper.Map<RecipeDto>(createdRecipe);

        // Return success with created recipe
        return OperationResult<RecipeDto>.Success(recipeDto);
    }
}