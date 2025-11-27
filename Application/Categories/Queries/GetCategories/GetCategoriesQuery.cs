using Application.Common.Models;
using Application.DTOs;
using MediatR;

namespace Application.Categories.Queries.GetCategories;

/// <summary>
/// Query to get all categories with recipe counts
/// </summary>
public class GetCategoriesQuery : IRequest<OperationResult<List<CategoryWithCountDto>>>
{
}