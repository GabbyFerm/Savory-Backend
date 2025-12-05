using FluentValidation;

namespace Application.Recipes.Commands.DeleteRecipe;

/// <summary>
/// Validator for DeleteRecipeCommand
/// </summary>
public class DeleteRecipeCommandValidator : AbstractValidator<DeleteRecipeCommand>
{
    public DeleteRecipeCommandValidator()
    {
        // Id validation
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Recipe ID is required");
    }
}