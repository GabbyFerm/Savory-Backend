using Application.Common.Models;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Categories.Queries.GetCategories;

/// <summary>
/// Handler for getting all categories with recipe counts
/// </summary>
public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, OperationResult<List<CategoryWithCountDto>>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetCategoriesQueryHandler(ICategoryRepository categoryRepository, ICurrentUserService currentUserService)
    {
        _categoryRepository = categoryRepository;
        _currentUserService = currentUserService;
    }

    public async Task<OperationResult<List<CategoryWithCountDto>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        // Get current user ID
        var userId = _currentUserService.GetUserId();
        if (userId == null)
        {
            return OperationResult<List<CategoryWithCountDto>>.Failure("User not authenticated");
        }

        // Get all categories
        var categories = await _categoryRepository.ListAsync();

        // Get recipe counts per category for current user
        var recipeCounts = await _categoryRepository.GetCategoryRecipeCountsAsync(userId.Value);

        // Map to DTOs with recipe counts
        var categoryDtos = categories.Select(c => new CategoryWithCountDto
        {
            Id = c.Id,
            Name = c.Name,
            RecipeCount = recipeCounts.ContainsKey(c.Id) ? recipeCounts[c.Id] : 0
        }).ToList();

        return OperationResult<List<CategoryWithCountDto>>.Success(categoryDtos);
    }
}