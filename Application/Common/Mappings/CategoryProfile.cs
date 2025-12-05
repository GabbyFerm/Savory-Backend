using Application.DTOs;
using Domain.Entities;

namespace Application.Common.Mappings;

/// <summary>
/// AutoMapper profile for Category entity mappings
/// </summary>
public class CategoryProfile : AutoMapper.Profile
{
    public CategoryProfile()
    {
        // Category -> CategoryDto
        CreateMap<Category, CategoryDto>();
    }
}