using Application.Common.Models;
using Application.DTOs;
using Application.TodoItems.Commands.CreateTodoItem;
using Application.TodoItems.Commands.DeleteTodoItem;
using Application.TodoItems.Commands.UpdateTodoItem;
using Application.TodoItems.Queries.GetTodoItemById;
using Application.TodoItems.Queries.GetTodoItems;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// API controller for managing todo items using MediatR CQRS pattern
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TodoController : ControllerBase
{
    private readonly IMediator _mediator;

    public TodoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets all todo items
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<OperationResult<IEnumerable<TodoItemDto>>>> GetAll()
    {
        var query = new GetTodoItemsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Gets a specific todo item by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<OperationResult<TodoItemDto>>> GetById(Guid id)
    {
        var query = new GetTodoItemByIdQuery(id);
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Creates a new todo item
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<OperationResult<TodoItemDto>>> Create(
        [FromBody] CreateTodoItemCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
    }

    /// <summary>
    /// Updates an existing todo item
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTodoItemCommand command)
    {
        // Ensure the ID in the route matches the command
        if (id != command.Id)
            return BadRequest(OperationResult.Failure("ID mismatch"));

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return NotFound(result);

        return NoContent();
    }

    /// <summary>
    /// Deletes a todo item
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteTodoItemCommand(id);
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return NotFound(result);

        return NoContent();
    }
}