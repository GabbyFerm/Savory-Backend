using Application.Common.Models;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Recipes.Queries.GetUserRecipes;

/// <summary>
/// Handler for getting all users recipes
/// </summary>
public class GetRecipesQueryHandler : IRequestHandler<GetRecipesQuery, OperationResult<PagedResult<RecipeDto>>>
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

    public async Task<OperationResult<PagedResult<RecipeDto>>> Handle(GetRecipesQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();
        if (userId == null)
        {
            return OperationResult<PagedResult<RecipeDto>>.Failure("User not authenticated");
        }

        var (recipes, totalCount) = await _recipeRepository.GetUserRecipesAsync(
            userId.Value,
            request.SearchTerm,
            request.CategoryId,
            request.SortBy,
            request.SortOrder,
            request.IngredientName,
            request.PageNumber,
            request.PageSize);

        var recipeDtos = _mapper.Map<IEnumerable<RecipeDto>>(recipes);

        var pagedResult = new PagedResult<RecipeDto>
        {
            Items = recipeDtos,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };

        return OperationResult<PagedResult<RecipeDto>>.Success(pagedResult);
    }
}