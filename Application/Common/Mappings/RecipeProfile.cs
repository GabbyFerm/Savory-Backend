using Application.DTOs;
using Domain.Entities;

namespace Application.Common.Mappings;

/// <summary>
/// AutoMapper profile for Recipe entity mappings
/// </summary>
public class RecipeProfile : AutoMapper.Profile
{
    public RecipeProfile()
    {
        // Recipe -> RecipeDto
        CreateMap<Recipe, RecipeDto>()
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.Ingredients,
                opt => opt.MapFrom(src => src.RecipeIngredients));

        // RecipeIngredient -> RecipeIngredientDto
        CreateMap<RecipeIngredient, RecipeIngredientDto>()
            .ForMember(dest => dest.IngredientName,
                opt => opt.MapFrom(src => src.Ingredient.Name))
            .ForMember(dest => dest.Unit,
                opt => opt.MapFrom(src => src.Ingredient.Unit));

        // RecipeCreateDto -> Recipe
        CreateMap<RecipeCreateDto, Recipe>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ImagePath, opt => opt.Ignore())
            .ForMember(dest => dest.RecipeIngredients, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());

        // RecipeUpdateDto -> Recipe
        CreateMap<RecipeUpdateDto, Recipe>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ImagePath, opt => opt.Ignore())
            .ForMember(dest => dest.RecipeIngredients, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());
    }
}