using Application.Common.Models;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Recipes.Queries.GetRecipeById;

/// <summary>
/// Handler for getting a single recipe by ID
/// </summary>
public class GetRecipeByIdQueryHandler : IRequestHandler<GetRecipeByIdQuery, OperationResult<RecipeDto>>
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetRecipeByIdQueryHandler(IRecipeRepository recipeRepository, IMapper mapper, ICurrentUserService currentUserService)
    {
        _recipeRepository = recipeRepository;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<OperationResult<RecipeDto>> Handle(GetRecipeByIdQuery request, CancellationToken cancellationToken)
    {
        // Get current user ID
        var userId = _currentUserService.GetUserId();
        if (userId == null)
        {
            return OperationResult<RecipeDto>.Failure("User not authenticated");
        }

        // Get recipe with full details (includes category and ingredients)
        var recipe = await _recipeRepository.GetRecipeWithDetailsAsync(request.Id);

        // Check if recipe exists
        if (recipe == null)
        {
            return OperationResult<RecipeDto>.Failure("Recipe not found");
        }

        // Authorization check - user can only view their own recipes
        if (recipe.UserId != userId.Value)
        {
            return OperationResult<RecipeDto>.Failure("You can only view your own recipes");
        }

        // Map to DTO
        var recipeDto = _mapper.Map<RecipeDto>(recipe);

        // Return success with data
        return OperationResult<RecipeDto>.Success(recipeDto);
    }
}