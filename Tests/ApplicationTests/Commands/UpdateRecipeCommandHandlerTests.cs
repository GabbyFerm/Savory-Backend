using Application.Interfaces;
using Application.Recipes.Commands.UpdateRecipe;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Tests.ApplicationTests.Commands;

public class UpdateRecipeCommandHandlerTests
{
    private readonly Mock<IRecipeRepository> _mockRecipeRepository;
    private readonly Mock<ICurrentUserService> _mockCurrentUserService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UpdateRecipeCommandHandler _handler;

    public UpdateRecipeCommandHandlerTests()
    {
        _mockRecipeRepository = new Mock<IRecipeRepository>();
        _mockCurrentUserService = new Mock<ICurrentUserService>();
        _mockMapper = new Mock<IMapper>();

        _handler = new UpdateRecipeCommandHandler(
            _mockRecipeRepository.Object,
            _mockMapper.Object,
            _mockCurrentUserService.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldUpdateRecipeAndReturnSuccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var recipeId = Guid.NewGuid();

        _mockCurrentUserService.Setup(x => x.GetUserId()).Returns(userId);

        var existingRecipe = new Recipe
        {
            Id = recipeId,
            UserId = userId,
            Title = "Old Title",
            RecipeIngredients = new List<RecipeIngredient>() // ← Already initialized
        };

        // Use GetRecipeWithDetailsAsync instead of GetByIdAsync
        _mockRecipeRepository.Setup(x => x.GetRecipeWithDetailsAsync(recipeId))
            .ReturnsAsync(existingRecipe);

        var command = new UpdateRecipeCommand
        {
            Id = recipeId,
            Title = "Updated Title",
            Description = "Updated Description",
            Instructions = "Updated Instructions",
            PrepTime = 15,
            CookTime = 25,
            Servings = 6,
            CategoryId = Guid.NewGuid(),
            Ingredients = new List<Application.DTOs.RecipeIngredientCreateDto>()
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        existingRecipe.Title.Should().Be("Updated Title");

        _mockRecipeRepository.Verify(x => x.UpdateAsync(existingRecipe), Times.Once);
        _mockRecipeRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_UserNotOwner_ShouldReturnFailure()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var recipeId = Guid.NewGuid();

        _mockCurrentUserService.Setup(x => x.GetUserId()).Returns(userId);

        var existingRecipe = new Recipe
        {
            Id = recipeId,
            UserId = otherUserId, // Different user!
            Title = "Old Title",
            RecipeIngredients = new List<RecipeIngredient>() // ← Already initialized
        };

        _mockRecipeRepository.Setup(x => x.GetRecipeWithDetailsAsync(recipeId))
            .ReturnsAsync(existingRecipe);

        var command = new UpdateRecipeCommand
        {
            Id = recipeId,
            Title = "Updated Title",
            Description = "Updated Description",
            Instructions = "Updated Instructions",
            PrepTime = 15,
            CookTime = 25,
            Servings = 6,
            CategoryId = Guid.NewGuid(),
            Ingredients = new List<Application.DTOs.RecipeIngredientCreateDto>()
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("only edit your own");

        _mockRecipeRepository.Verify(x => x.UpdateAsync(It.IsAny<Recipe>()), Times.Never);
    }
}