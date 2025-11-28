using Application.Interfaces;
using Application.Recipes.Queries.GetRecipeById;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Tests.ApplicationTests.Queries;

public class GetRecipeByIdQueryHandlerTests
{
    private readonly Mock<IRecipeRepository> _mockRecipeRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ICurrentUserService> _mockCurrentUserService;
    private readonly GetRecipeByIdQueryHandler _handler;

    public GetRecipeByIdQueryHandlerTests()
    {
        _mockRecipeRepository = new Mock<IRecipeRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockCurrentUserService = new Mock<ICurrentUserService>();

        _handler = new GetRecipeByIdQueryHandler(
            _mockRecipeRepository.Object,
            _mockMapper.Object,
            _mockCurrentUserService.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldReturnRecipe()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var recipeId = Guid.NewGuid();

        _mockCurrentUserService.Setup(x => x.GetUserId()).Returns(userId);

        var recipe = new Recipe
        {
            Id = recipeId,
            UserId = userId,
            Title = "Test Recipe"
        };

        _mockRecipeRepository.Setup(x => x.GetRecipeWithDetailsAsync(recipeId))
            .ReturnsAsync(recipe);

        _mockMapper.Setup(x => x.Map<Application.DTOs.RecipeDto>(recipe))
            .Returns(new Application.DTOs.RecipeDto { Id = recipeId, Title = "Test Recipe" });

        var query = new GetRecipeByIdQuery { Id = recipeId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Title.Should().Be("Test Recipe");
    }

    [Fact]
    public async Task Handle_UserNotOwner_ShouldReturnFailure()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var recipeId = Guid.NewGuid();

        _mockCurrentUserService.Setup(x => x.GetUserId()).Returns(userId);

        var recipe = new Recipe
        {
            Id = recipeId,
            UserId = otherUserId, // Different user!
            Title = "Private Recipe"
        };

        _mockRecipeRepository.Setup(x => x.GetRecipeWithDetailsAsync(recipeId))
            .ReturnsAsync(recipe);

        var query = new GetRecipeByIdQuery { Id = recipeId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("only view your own");
    }
}