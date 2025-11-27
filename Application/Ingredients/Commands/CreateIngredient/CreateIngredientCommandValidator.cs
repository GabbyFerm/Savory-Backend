using FluentValidation;

namespace Application.Ingredients.Commands.CreateIngredient;

/// <summary>
/// Validator for CreateIngredientCommand
/// </summary>
public class CreateIngredientCommandValidator : AbstractValidator<CreateIngredientCommand>
{
    public CreateIngredientCommandValidator()
    {
        // Name validation
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ingredient name is required")
            .MaximumLength(100).WithMessage("Ingredient name must not exceed 100 characters");

        // Unit validation
        RuleFor(x => x.Unit)
            .NotEmpty().WithMessage("Unit is required")
            .MaximumLength(20).WithMessage("Unit must not exceed 20 characters")
            .Must(BeValidUnit).WithMessage("Unit must be one of: g, kg, ml, l, pcs, tbsp, tsp, cup");
    }

    private bool BeValidUnit(string unit)
    {
        var validUnits = new[] { "g", "kg", "ml", "l", "pcs", "tbsp", "tsp", "cup", "oz", "lb" };
        return validUnits.Contains(unit.ToLower());
    }
}