using Application.Common.Models;
using MediatR;

namespace Application.Recipes.Commands.UploadRecipeImage;

/// <summary>
/// Command to upload an image for a recipe
/// </summary>
public class UploadRecipeImageCommand : IRequest<OperationResult<string>>
{
    /// <summary>
    /// Recipe ID
    /// </summary>
    public Guid RecipeId { get; set; }

    /// <summary>
    /// Image file stream
    /// </summary>
    public Stream ImageStream { get; set; } = null!;

    /// <summary>
    /// Original file name
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// File size in bytes
    /// </summary>
    public long FileSize { get; set; }
}