using Application.Common.Models;
using Application.DTOs;
using MediatR;

namespace Application.TodoItems.Commands.CreateTodoItem;

public record CreateTodoItemCommand : IRequest<OperationResult<TodoItemDto>>
{
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTime? DueDate { get; init; }
}