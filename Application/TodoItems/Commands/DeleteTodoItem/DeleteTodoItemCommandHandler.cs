using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.TodoItems.Commands.DeleteTodoItem;

public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteTodoItemCommand, OperationResult>
{
    private readonly IGenericRepository<TodoItem> _repository;

    public DeleteTodoItemCommandHandler(IGenericRepository<TodoItem> repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult> Handle(
        DeleteTodoItemCommand request,
        CancellationToken cancellationToken)
    {
        var todoItem = await _repository.GetByIdAsync(request.Id);

        if (todoItem == null)
        {
            return OperationResult.Failure($"TodoItem with ID {request.Id} not found");
        }

        await _repository.DeleteAsync(request.Id);
        await _repository.SaveChangesAsync();

        return OperationResult.Success();
    }
}