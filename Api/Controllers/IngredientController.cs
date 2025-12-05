using Application.Ingredients.Commands.CreateIngredient;
using Application.Ingredients.Queries.GetIngredientById;
using Application.Ingredients.Queries.GetIngredients;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Ingredient management endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class IngredientController : ControllerBase
{
    private readonly IMediator _mediator;

    public IngredientController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets all available ingredients with optional search
    /// </summary>
    /// <param name="searchTerm">Optional search term to filter by name</param>
    /// <returns>List of ingredients</returns>
    [HttpGet]
    public async Task<IActionResult> GetIngredients([FromQuery] string? searchTerm)
    {
        var query = new GetIngredientsQuery
        {
            SearchTerm = searchTerm
        };

        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            return BadRequest(new { Message = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Gets a single ingredient by ID
    /// </summary>
    /// <param name="id">Ingredient ID</param>
    /// <returns>Ingredient details</returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetIngredientById(Guid id)
    {
        var query = new GetIngredientByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            return NotFound(new { Message = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new ingredient
    /// </summary>
    /// <param name="command">Ingredient creation data</param>
    /// <returns>Created ingredient</returns>
    [HttpPost]
    public async Task<IActionResult> CreateIngredient([FromBody] CreateIngredientCommand command)
    {
        if (command == null)
        {
            return BadRequest(new { Message = "Invalid request body" });
        }

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(new { Message = result.ErrorMessage });
        }

        return CreatedAtAction(
            nameof(GetIngredientById),
            new { id = result.Data!.Id },
            result.Data);
    }
}