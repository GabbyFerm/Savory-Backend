using Application.Common.Models;
using Application.DTOs;
using MediatR;

namespace Application.TodoItems.Queries.GetTodoItemById;

public record GetTodoItemByIdQuery(Guid Id) : IRequest<OperationResult<TodoItemDto>>;