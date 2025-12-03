using FluentValidation;

namespace Application.Recipes.Commands.CreateRecipe;

/// <summary>
/// Validator for CreateRecipeCommand
/// </summary>
public class CreateRecipeCommandValidator : AbstractValidator<CreateRecipeCommand>
{
    public CreateRecipeCommandValidator()
    {
        // Title validation
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        // Description validation (optional)
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters");

        // Instructions validation
        RuleFor(x => x.Instructions)
            .NotEmpty().WithMessage("Instructions are required")
            .MaximumLength(5000).WithMessage("Instructions must not exceed 5000 characters");

        // PrepTime validation
        RuleFor(x => x.PrepTime)
            .GreaterThanOrEqualTo(0).WithMessage("Prep time cannot be negative");

        // CookTime validation
        RuleFor(x => x.CookTime)
            .GreaterThanOrEqualTo(0).WithMessage("Cook time cannot be negative");

        // Servings validation
        RuleFor(x => x.Servings)
            .GreaterThan(0).WithMessage("Servings must be at least 1");

        // Category validation
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

            // Allow zero quantity for "to taste" ingredients
            ingredient.RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative");
        });
    }
}