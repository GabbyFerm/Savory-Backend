using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings;

/// <summary>
/// AutoMapper profile for Ingredient entity mappings
/// </summary>
public class IngredientProfile : Profile
{
    public IngredientProfile()
    {
        // Ingredient -> IngredientDto
        CreateMap<Ingredient, IngredientDto>();

        // IngredientDto -> Ingredient (for creating new ingredients)
        CreateMap<IngredientDto, Ingredient>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.RecipeIngredients, opt => opt.Ignore());
    }
}