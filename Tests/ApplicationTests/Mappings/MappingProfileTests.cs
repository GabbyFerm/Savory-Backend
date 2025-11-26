using Application.Common.Mappings;
using Application.DTOs;
using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;

namespace Tests.ApplicationTests.Mappings;

public class MappingProfileTests
{
    private readonly IMapper _mapper;

    public MappingProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RecipeProfile>();
            cfg.AddProfile<IngredientProfile>();
            cfg.AddProfile<CategoryProfile>();
            cfg.AddProfile<UserProfile>();
        });

        _mapper = configuration.CreateMapper();
    }
        
    [Fact]
    public void AutoMapper_Configuration_IsValid()
    {
        // Assert - will throw if configuration is invalid
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    [Fact]
    public void Should_Map_Recipe_To_RecipeDto()
    {
        // Arrange
        var recipe = new Recipe
        {
            Id = Guid.NewGuid(),
            Title = "Test Recipe",
            Description = "Test Description",
            Instructions = "Test Instructions",
            PrepTime = 10,
            CookTime = 20,
            Servings = 4,
            CategoryId = Guid.NewGuid(),
            Category = new Category { Id = Guid.NewGuid(), Name = "Breakfast" },
            CreatedAt = DateTime.UtcNow,
            RecipeIngredients = new List<RecipeIngredient>()
        };

        // Act
        var dto = _mapper.Map<RecipeDto>(recipe);

        // Assert
        dto.Should().NotBeNull();
        dto.Title.Should().Be(recipe.Title);
        dto.CategoryName.Should().Be("Breakfast");
    }

    [Fact]
    public void Should_Map_Ingredient_To_IngredientDto()
    {
        // Arrange
        var ingredient = new Ingredient
        {
            Id = Guid.NewGuid(),
            Name = "Flour",
            Unit = "g",
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var dto = _mapper.Map<IngredientDto>(ingredient);

        // Assert
        dto.Should().NotBeNull();
        dto.Name.Should().Be("Flour");
        dto.Unit.Should().Be("g");
    }
}