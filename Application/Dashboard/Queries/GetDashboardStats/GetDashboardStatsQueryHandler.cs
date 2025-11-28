using Application.Common.Models;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Dashboard.Queries.GetDashboardStats;

/// <summary>
/// Handler for getting dashboard statistics
/// </summary>
public class GetDashboardStatsQueryHandler : IRequestHandler<GetDashboardStatsQuery, OperationResult<DashboardStatsDto>>
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetDashboardStatsQueryHandler(IRecipeRepository recipeRepository, ICategoryRepository categoryRepository, IMapper mapper, ICurrentUserService currentUserService)
    {
        _recipeRepository = recipeRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<OperationResult<DashboardStatsDto>> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        // Get current user ID
        var userId = _currentUserService.GetUserId();
        if (userId == null)
        {
            return OperationResult<DashboardStatsDto>.Failure("user not authenticated");
        }

        // Get statistics using repository methods
        var totalRecipes = await _recipeRepository.GetUserRecipeCountAsync(userId.Value);
        var recipesByCategory = await _recipeRepository.GetRecipeCountByCategoryAsync(userId.Value);
        var avgCookTime = await _recipeRepository.GetAverageCookTimeAsync(userId.Value);
        var avgTotalTime = await _recipeRepository.GetAverageTotalTimeAsync(userId.Value);
        var recentRecipes = await _recipeRepository.GetRecentRecipesAsync(userId.Value, 5);

        // Get all categories to map IDs to names
        var categories = await _categoryRepository.ListAsync();
        var categoryDict = categories.ToDictionary(c => c.Id, c => c.Name);

        // Map category IDs to names
        var recipesByCategoryName = recipesByCategory
            .Where(kvp => categoryDict.ContainsKey(kvp.Key))
            .ToDictionary(
                kvp => categoryDict[kvp.Key],
                kvp => kvp.Value
            );

        // Create DTO
        var stats = new DashboardStatsDto
        {
            TotalRecipes = totalRecipes,
            RecipesByCategory = recipesByCategoryName,
            AverageCookTime = Math.Round(avgCookTime, 1),
            AverageTotalTime = Math.Round(avgTotalTime, 1),
            RecentRecipes = _mapper.Map<List<RecipeDto>>(recentRecipes)
        };

        return OperationResult<DashboardStatsDto>.Success(stats);
    }
}