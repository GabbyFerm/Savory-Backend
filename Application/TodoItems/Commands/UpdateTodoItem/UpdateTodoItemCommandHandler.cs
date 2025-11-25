using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.TodoItems.Commands.UpdateTodoItem;

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateTodoItemCommand, OperationResult>
{
    private readonly IGenericRepository<TodoItem> _repository;

    public UpdateTodoItemCommandHandler(IGenericRepository<TodoItem> repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult> Handle(
        UpdateTodoItemCommand request,
        CancellationToken cancellationToken)
    {
        var todoItem = await _repository.GetByIdAsync(request.Id);

        if (todoItem == null)
        {
            return OperationResult.Failure($"TodoItem with ID {request.Id} not found");
        }

        todoItem.Title = request.Title;
        todoItem.Description = request.Description;
        todoItem.IsCompleted = request.IsCompleted;
        todoItem.DueDate = request.DueDate;
        todoItem.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(todoItem);
        await _repository.SaveChangesAsync();

        return OperationResult.Success();
    }
}