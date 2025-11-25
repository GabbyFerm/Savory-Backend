using Application.Interfaces;
using Application.TodoItems.Commands.CreateTodoItem;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Tests.ApplicationTests.Commands;

/// <summary>
/// Unit tests for CreateTodoItemCommandHandler
/// </summary>
public class CreateTodoItemCommandHandlerTests
{
    private readonly Mock<IGenericRepository<TodoItem>> _mockRepository;
    private readonly CreateTodoItemCommandHandler _handler;

    public CreateTodoItemCommandHandlerTests()
    {
        _mockRepository = new Mock<IGenericRepository<TodoItem>>();
        _handler = new CreateTodoItemCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateTodoItem()
    {
        // Arrange
        var command = new CreateTodoItemCommand
        {
            Title = "Test Todo",
            Description = "Test Description",
            DueDate = DateTime.UtcNow.AddDays(7)
        };

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<TodoItem>()))
            .ReturnsAsync((TodoItem item) => item);
        _mockRepository.Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Title.Should().Be("Test Todo");
        result.Data.Description.Should().Be("Test Description");
        result.Data.IsCompleted.Should().BeFalse();

        _mockRepository.Verify(r => r.AddAsync(It.IsAny<TodoItem>()), Times.Once);
        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}