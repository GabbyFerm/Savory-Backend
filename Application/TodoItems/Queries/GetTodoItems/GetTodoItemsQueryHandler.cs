using Application.Common.Mappings;
using Application.Common.Models;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.TodoItems.Queries.GetTodoItems;

public class GetTodoItemsQueryHandler
    : IRequestHandler<GetTodoItemsQuery, OperationResult<IEnumerable<TodoItemDto>>>
{
    private readonly IGenericRepository<TodoItem> _repository;

    public GetTodoItemsQueryHandler(IGenericRepository<TodoItem> repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<IEnumerable<TodoItemDto>>> Handle(
        GetTodoItemsQuery request,
        CancellationToken cancellationToken)
    {
        var todoItems = await _repository.ListAsync();
        var dtos = todoItems.Select(t => t.ToDto()).ToList();

        return OperationResult<IEnumerable<TodoItemDto>>.Success(dtos);
    }
}