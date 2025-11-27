using FluentValidation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace Application.Recipes.Commands.UpdateRecipe;

/// <summary>
/// Validator for UpdateRecipeCommand
/// </summary>
public class UpdateRecipeCommandValidator : AbstractValidator<UpdateRecipeCommand>
{
    public UpdateRecipeCommandValidator()
    {
        // Id validation
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Recipe ID is required");

        // Title validation
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        // Description validation
        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        // Instructions validation
        RuleFor(x => x.Instructions)
            .NotEmpty().WithMessage("Instructions are required");

        // PrepTime validation
        RuleFor(x => x.PrepTime)
            .GreaterThanOrEqualTo(0).WithMessage("Preparation time must be 0 or greater");

        // CookTime validation
        RuleFor(x => x.CookTime)
            .GreaterThan(0).WithMessage("Cook time must be greater than 0");

        // Servings validation
        RuleFor(x => x.Servings)
            .GreaterThan(0).WithMessage("Servings must be greater than 0");

        // CategoryId validation
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category is required");

        // Ingredients validation
        RuleFor(x => x.Ingredients)
            .NotEmpty().WithMessage("At least one ingredient is required");

        // Validate each ingredient
        RuleForEach(x => x.Ingredients).ChildRules(ingredient =>
        {
            ingredient.RuleFor(x => x.IngredientId)
                .NotEmpty().WithMessage("Ingredient ID is required");

            ingredient.RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");
        });
    }
}