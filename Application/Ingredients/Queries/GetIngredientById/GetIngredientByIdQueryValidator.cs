using FluentValidation;

namespace Application.Ingredients.Queries.GetIngredientById;

/// <summary>
/// Validator for GetIngredientByIdQuery
/// </summary>
public class GetIngredientByIdQueryValidator : AbstractValidator<GetIngredientByIdQuery>
{
    public GetIngredientByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Ingredient ID is required");
    }
}