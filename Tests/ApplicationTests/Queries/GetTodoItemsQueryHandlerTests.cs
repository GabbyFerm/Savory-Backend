using Application.Interfaces;
using Application.TodoItems.Queries.GetTodoItems;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Tests.ApplicationTests.Queries;

/// <summary>
/// Unit tests for GetTodoItemsQueryHandler
/// </summary>
public class GetTodoItemsQueryHandlerTests
{
    private readonly Mock<IGenericRepository<TodoItem>> _mockRepository;
    private readonly GetTodoItemsQueryHandler _handler;

    public GetTodoItemsQueryHandlerTests()
    {
        _mockRepository = new Mock<IGenericRepository<TodoItem>>();
        _handler = new GetTodoItemsQueryHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllTodoItems()
    {
        // Arrange
        var todoItems = new List<TodoItem>
        {
            new() { Id = Guid.NewGuid(), Title = "Todo 1", CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Title = "Todo 2", CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Title = "Todo 3", CreatedAt = DateTime.UtcNow }
        };

        _mockRepository.Setup(r => r.ListAsync())
            .ReturnsAsync(todoItems);

        var query = new GetTodoItemsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(3);
        result.Data.Should().Contain(dto => dto.Title == "Todo 1");

        _mockRepository.Verify(r => r.ListAsync(), Times.Once);
    }
}