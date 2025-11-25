using Application.Common.Models;
using MediatR;

namespace Application.TodoItems.Commands.UpdateTodoItem;

public record UpdateTodoItemCommand : IRequest<OperationResult>
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsCompleted { get; init; }
    public DateTime? DueDate { get; init; }
}