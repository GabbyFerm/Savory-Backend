using Application.TodoItems.Commands.CreateTodoItem;
using FluentAssertions;

namespace Tests.ApplicationTests.Validators;

/// <summary>
/// Unit tests for CreateTodoItemCommandValidator
/// </summary>
public class CreateTodoItemCommandValidatorTests
{
    private readonly CreateTodoItemCommandValidator _validator;

    public CreateTodoItemCommandValidatorTests()
    {
        _validator = new CreateTodoItemCommandValidator();
    }

    [Fact]
    public void Validate_ValidCommand_ShouldNotHaveErrors()
    {
        // Arrange
        var command = new CreateTodoItemCommand
        {
            Title = "Valid Todo",
            Description = "Valid Description",
            DueDate = DateTime.UtcNow.AddDays(1)
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_EmptyTitle_ShouldHaveError()
    {
        // Arrange
        var command = new CreateTodoItemCommand
        {
            Title = "",
            Description = "Description"
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title");
    }

    [Fact]
    public void Validate_TitleTooLong_ShouldHaveError()
    {
        // Arrange
        var command = new CreateTodoItemCommand
        {
            Title = new string('a', 201), // 201 characters
            Description = "Description"
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title");
    }

    [Fact]
    public void Validate_PastDueDate_ShouldHaveError()
    {
        // Arrange
        var command = new CreateTodoItemCommand
        {
            Title = "Todo",
            DueDate = DateTime.UtcNow.AddDays(-1) // Past date
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "DueDate");
    }
}