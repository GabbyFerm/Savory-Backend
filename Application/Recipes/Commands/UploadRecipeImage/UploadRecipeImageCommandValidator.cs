using FluentValidation;

namespace Application.Recipes.Commands.UploadRecipeImage;

/// <summary>
/// Validator for UploadRecipeImageCommand
/// </summary>
public class UploadRecipeImageCommandValidator : AbstractValidator<UploadRecipeImageCommand>
{
    public UploadRecipeImageCommandValidator()
    {
        RuleFor(x => x.RecipeId)
            .NotEmpty().WithMessage("Recipe ID is required");

        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("File name is required");

        RuleFor(x => x.FileSize)
            .GreaterThan(0).WithMessage("File is empty");
    }
}