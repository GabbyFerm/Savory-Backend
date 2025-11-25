using Application.Common.Models;
using Application.DTOs;
using MediatR;

namespace Application.TodoItems.Queries.GetTodoItems;

public record GetTodoItemsQuery : IRequest<OperationResult<IEnumerable<TodoItemDto>>>;