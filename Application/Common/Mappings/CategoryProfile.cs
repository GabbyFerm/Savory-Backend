using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

/// <summary>
/// AutoMapper profile for Category entity mappings
/// </summary>
public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        // Category -> CategoryDto
        CreateMap<Category, CategoryDto>();
    }
}