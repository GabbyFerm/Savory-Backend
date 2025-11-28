using Application.Interfaces;
using Application.Recipes.Commands.CreateRecipe;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Tests.ApplicationTests.Commands;

public class CreateRecipeCommandHandlerTests
{
    private readonly Mock<IRecipeRepository> _mockRecipeRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ICurrentUserService> _mockCurrentUserService;
    private readonly CreateRecipeCommandHandler _handler;

    public CreateRecipeCommandHandlerTests()
    {
        _mockRecipeRepository = new Mock<IRecipeRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockCurrentUserService = new Mock<ICurrentUserService>();

        _handler = new CreateRecipeCommandHandler(
            _mockRecipeRepository.Object,
            _mockMapper.Object,
            _mockCurrentUserService.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldCreateRecipeAndReturnSuccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var recipeId = Guid.NewGuid();

        _mockCurrentUserService.Setup(x => x.GetUserId()).Returns(userId);

        var command = new CreateRecipeCommand
        {
            Title = "Test Recipe",
            Description = "Test Description",
            Instructions = "Test Instructions",
            PrepTime = 10,
            CookTime = 20,
            Servings = 4,
            CategoryId = Guid.NewGuid(),
            Ingredients = new List<Application.DTOs.RecipeIngredientCreateDto>
            {
                new() { IngredientId = Guid.NewGuid(), Quantity = 100 }
            }
        };

        var recipe = new Recipe
        {
            Id = recipeId,
            UserId = userId,
            Title = command.Title
        };

        _mockRecipeRepository.Setup(x => x.GetRecipeWithDetailsAsync(It.IsAny<Guid>()))
            .ReturnsAsync(recipe);

        _mockMapper.Setup(x => x.Map<Application.DTOs.RecipeDto>(It.IsAny<Recipe>()))
            .Returns(new Application.DTOs.RecipeDto { Id = recipeId, Title = command.Title });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Title.Should().Be(command.Title);

        _mockRecipeRepository.Verify(x => x.AddAsync(It.IsAny<Recipe>()), Times.Once);
        _mockRecipeRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_UserNotAuthenticated_ShouldReturnFailure()
    {
        // Arrange
        _mockCurrentUserService.Setup(x => x.GetUserId()).Returns((Guid?)null);

        var command = new CreateRecipeCommand
        {
            Title = "Test Recipe",
            Description = "Test Description",
            Instructions = "Test Instructions",
            PrepTime = 10,
            CookTime = 20,
            Servings = 4,
            CategoryId = Guid.NewGuid(),
            Ingredients = new List<Application.DTOs.RecipeIngredientCreateDto>()
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("User not authenticated");

        _mockRecipeRepository.Verify(x => x.AddAsync(It.IsAny<Recipe>()), Times.Never);
    }
}