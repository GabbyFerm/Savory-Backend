using Application.Common.Mappings;
using Application.Common.Models;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.TodoItems.Queries.GetTodoItemById;

public class GetTodoItemByIdQueryHandler : IRequestHandler<GetTodoItemByIdQuery, OperationResult<TodoItemDto>>
{
    private readonly IGenericRepository<TodoItem> _repository;

    public GetTodoItemByIdQueryHandler(IGenericRepository<TodoItem> repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<TodoItemDto>> Handle(
        GetTodoItemByIdQuery request,
        CancellationToken cancellationToken)
    {
        var todoItem = await _repository.GetByIdAsync(request.Id);

        if (todoItem == null)
        {
            return OperationResult<TodoItemDto>.Failure($"TodoItem with ID {request.Id} not found");
        }

        return OperationResult<TodoItemDto>.Success(todoItem.ToDto());
    }
}