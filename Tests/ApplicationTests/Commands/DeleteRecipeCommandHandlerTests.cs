using Application.Interfaces;
using Application.Recipes.Commands.DeleteRecipe;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Tests.ApplicationTests.Commands;

public class DeleteRecipeCommandHandlerTests
{
    private readonly Mock<IRecipeRepository> _mockRecipeRepository;
    private readonly Mock<ICurrentUserService> _mockCurrentUserService;
    private readonly DeleteRecipeCommandHandler _handler;

    public DeleteRecipeCommandHandlerTests()
    {
        _mockRecipeRepository = new Mock<IRecipeRepository>();
        _mockCurrentUserService = new Mock<ICurrentUserService>();

        _handler = new DeleteRecipeCommandHandler(
            _mockRecipeRepository.Object,
            _mockCurrentUserService.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldDeleteRecipeAndReturnSuccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var recipeId = Guid.NewGuid();

        _mockCurrentUserService.Setup(x => x.GetUserId()).Returns(userId);

        var existingRecipe = new Recipe
        {
            Id = recipeId,
            UserId = userId,
            Title = "Recipe to Delete"
        };

        _mockRecipeRepository.Setup(x => x.GetByIdAsync(recipeId))
            .ReturnsAsync(existingRecipe);

        var command = new DeleteRecipeCommand { Id = recipeId };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        _mockRecipeRepository.Verify(x => x.DeleteAsync(recipeId), Times.Once);
        _mockRecipeRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_RecipeNotFound_ShouldReturnFailure()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var recipeId = Guid.NewGuid();

        _mockCurrentUserService.Setup(x => x.GetUserId()).Returns(userId);

        _mockRecipeRepository.Setup(x => x.GetByIdAsync(recipeId))
            .ReturnsAsync((Recipe?)null);

        var command = new DeleteRecipeCommand { Id = recipeId };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("not found");

        _mockRecipeRepository.Verify(x => x.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }
}