using Application.Common.Models;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Recipes.Queries.GetUserRecipes;

/// <summary>
/// Handler for getting all users recipes
/// </summary>
public class GetRecipesQueryHandler : IRequestHandler<GetRecipesQuery, OperationResult<IEnumerable<RecipeDto>>>
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetRecipesQueryHandler(IRecipeRepository recipeRepository, IMapper mapper, ICurrentUserService currentUserService)
    {
        _recipeRepository = recipeRepository;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<OperationResult<IEnumerable<RecipeDto>>> Handle(GetRecipesQuery request, CancellationToken cancellationToken)
    {
        // Get current user ID
        var userId = _currentUserService.GetUserId();
        if (userId == null)
        {
            return OperationResult<IEnumerable<RecipeDto>>.Failure("User not authenticated");
        }

        var userRecipes = await _recipeRepository.GetUserRecipesAsync(
            userId.Value,
            request.SearchTerm,
            request.CategoryId,
            request.SortBy,
            request.SortOrder,
            request.IngredientName);

        // Map to DTO
        var recipeDtos = _mapper.Map<IEnumerable<RecipeDto>>(userRecipes);

        // Return success with created recipe
        return OperationResult<IEnumerable<RecipeDto>>.Success(recipeDtos);
    }
}