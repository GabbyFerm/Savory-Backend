using Application.Common.Models;
using MediatR;

namespace Application.TodoItems.Commands.DeleteTodoItem;

public record DeleteTodoItemCommand(Guid Id) : IRequest<OperationResult>;