using FluentValidation;

namespace Application.Recipes.Queries.GetRecipeById;

/// <summary>
/// Validator for GetRecipeByIdQuery
/// </summary>
public class GetRecipeByIdQueryValidator : AbstractValidator<GetRecipeByIdQuery>
{
    public GetRecipeByIdQueryValidator()
    {
        // Id validation
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Recipe ID is required");
    }
}