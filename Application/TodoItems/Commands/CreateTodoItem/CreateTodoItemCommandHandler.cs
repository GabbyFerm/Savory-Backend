using Application.Common.Mappings;
using Application.Common.Models;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.TodoItems.Commands.CreateTodoItem;

public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, OperationResult<TodoItemDto>>
{
    private readonly IGenericRepository<TodoItem> _repository;

    public CreateTodoItemCommandHandler(IGenericRepository<TodoItem> repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<TodoItemDto>> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = new TodoItem
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            IsCompleted = false,
            DueDate = request.DueDate,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(todoItem);
        await _repository.SaveChangesAsync();

        return OperationResult<TodoItemDto>.Success(todoItem.ToDto());
    }
}