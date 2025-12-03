using Application.Recipes.Commands.CreateRecipe;
using Application.Recipes.Commands.DeleteRecipe;
using Application.Recipes.Commands.UpdateRecipe;
using Application.Recipes.Commands.UploadRecipeImage;
using Application.Recipes.Queries.GetRecipeById;
using Application.Recipes.Queries.GetUserRecipes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Recipe management endpoints
/// </summary>

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class RecipeController : ControllerBase
{
    private readonly IMediator _mediator;

    public RecipeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets all recipes for the current user with optional filtering and sorting
    /// </summary>
    /// <param name="searchTerm">Search term for recipe title</param>
    /// <param name="categoryId">Filter by category ID</param>
    /// <param name="sortBy">Sort by: title, cooktime, createddate</param>
    /// <param name="sortOrder">Sort order: asc, desc</param>
    /// <param name="ingredientName">Filter by ingredient name</param>
    /// <returns>List of recipes</returns>
    [HttpGet]
    public async Task<IActionResult> GetRecipes(
        [FromQuery] string? searchTerm,
        [FromQuery] Guid? categoryId,
        [FromQuery] string? sortBy,
        [FromQuery] string? sortOrder,
        [FromQuery] string? ingredientName,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new GetRecipesQuery
        {
            SearchTerm = searchTerm,
            CategoryId = categoryId,
            SortBy = sortBy,
            SortOrder = sortOrder,
            IngredientName = ingredientName,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            return BadRequest(new { Message = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Gets a single recipe by ID
    /// </summary>
    /// <param name="id">Recipe ID</param>
    /// <returns>Recipe details with ingredients</returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRecipeById(Guid id)
    {
        var query = new GetRecipeByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            return NotFound(new { Message = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new recipe
    /// </summary>
    /// <param name="command">Recipe creation data</param>
    /// <returns>Created recipe</returns>
    [HttpPost]
    public async Task<IActionResult> CreateRecipe([FromBody] CreateRecipeCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(new { Message = result.ErrorMessage });
        }

        return CreatedAtAction(
            nameof(GetRecipeById),
            new { id = result.Data!.Id },
            result.Data);
    }

    /// <summary>
    /// Uploads an image for a recipe
    /// </summary>
    /// <param name="id">Recipe ID</param>
    /// <param name="file">Image file</param>
    /// <returns>Image URL</returns>
    [HttpPost("{id:guid}/image")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadRecipeImage(Guid id, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { Message = "No file uploaded" });
        }

        var command = new UploadRecipeImageCommand
        {
            RecipeId = id,
            ImageStream = file.OpenReadStream(),
            FileName = file.FileName,
            FileSize = file.Length
        };

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(new { Message = result.ErrorMessage });
        }

        return Ok(new { ImageUrl = result.Data });
    }

    /// <summary>
    /// Updates an existing recipe
    /// </summary>
    /// <param name="id">Recipe ID</param>
    /// <param name="command">Updated recipe data</param>
    /// <returns>Updated recipe</returns>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateRecipe(Guid id, [FromBody] UpdateRecipeCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest(new { Message = "Route ID does not match body ID" });
        }

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(new { Message = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Deletes a recipe
    /// </summary>
    /// <param name="id">Recipe ID</param>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRecipe(Guid id)
    {
        var command = new DeleteRecipeCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return NotFound(new { Message = result.ErrorMessage });
        }

        return NoContent();
    }
}