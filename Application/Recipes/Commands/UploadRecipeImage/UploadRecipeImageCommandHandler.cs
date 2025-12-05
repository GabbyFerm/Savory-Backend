using Application.Common.Models;
using Application.Interfaces;
using MediatR;

namespace Application.Recipes.Commands.UploadRecipeImage;

/// <summary>
/// Handler for uploading recipe images
/// </summary>
public class UploadRecipeImageCommandHandler : IRequestHandler<UploadRecipeImageCommand, OperationResult<string>>
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IImageService _imageService;
    private readonly ICurrentUserService _currentUserService;

    public UploadRecipeImageCommandHandler(IRecipeRepository recipeRepository, IImageService imageService, ICurrentUserService currentUserService)
    {
        _recipeRepository = recipeRepository;
        _imageService = imageService;
        _currentUserService = currentUserService;
    }

    public async Task<OperationResult<string>> Handle(UploadRecipeImageCommand request, CancellationToken cancellationToken)
    {
        // Get current user ID
        var userId = _currentUserService.GetUserId();
        if (userId == null)
        {
            return OperationResult<string>.Failure("User not authenticated");
        }

        // Validate image
        if (!_imageService.ValidateImage(request.FileName, request.FileSize))
        {
            return OperationResult<string>.Failure(
                "Invalid image file. Allowed formats: jpg, jpeg, png, webp. Max size: 5MB");
        }

        // Load recipe
        var recipe = await _recipeRepository.GetByIdAsync(request.RecipeId);
        if (recipe == null)
        {
            return OperationResult<string>.Failure("Recipe not found");
        }

        // Authorization check
        if (recipe.UserId != userId.Value)
        {
            return OperationResult<string>.Failure("You can only upload images to your own recipes");
        }

        // Delete old image if exists
        if (!string.IsNullOrWhiteSpace(recipe.ImagePath))
        {
            await _imageService.DeleteImageAsync(recipe.ImagePath);
        }

        // Save new image
        var imagePath = await _imageService.SaveImageAsync(request.ImageStream, request.FileName);

        // Update recipe with image path
        recipe.ImagePath = imagePath;
        recipe.UpdatedAt = DateTime.UtcNow;

        await _recipeRepository.UpdateAsync(recipe);
        await _recipeRepository.SaveChangesAsync();

        // Return image URL
        return OperationResult<string>.Success(imagePath);
    }
}