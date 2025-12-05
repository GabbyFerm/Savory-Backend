using FluentValidation;

namespace Application.Recipes.Queries.GetUserRecipes;

/// <summary>
/// Validator for GetRecipesQuery
/// </summary>
public class GetRecipesQueryValidator : AbstractValidator<GetRecipesQuery>
{
    public GetRecipesQueryValidator()
    {
        // PageNumber validation
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1");

        // PageSize validation
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("Page size must be at least 1")
            .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100");

        // SearchTerm validation (optional)
        RuleFor(x => x.SearchTerm)
            .MaximumLength(200).WithMessage("Search term must not exceed 200 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm));

        // IngredientName validation (optional)
        RuleFor(x => x.IngredientName)
            .MaximumLength(100).WithMessage("Ingredient name must not exceed 100 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.IngredientName));

        // SortBy validation (optional)
        RuleFor(x => x.SortBy)
            .Must(BeValidSortField).WithMessage("Invalid sort field. Valid options: title, categoryname, createddate, cooktime")
            .When(x => !string.IsNullOrWhiteSpace(x.SortBy));

        // SortOrder validation (optional)
        RuleFor(x => x.SortOrder)
            .Must(BeValidSortOrder).WithMessage("Invalid sort order. Valid options: asc, desc")
            .When(x => !string.IsNullOrWhiteSpace(x.SortOrder));
    }

    private bool BeValidSortField(string? sortBy)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
            return true;

        var validFields = new[] { "title", "categoryname", "createddate", "cooktime" };
        return validFields.Contains(sortBy.ToLower());
    }

    private bool BeValidSortOrder(string? sortOrder)
    {
        if (string.IsNullOrWhiteSpace(sortOrder))
            return true;

        var validOrders = new[] { "asc", "desc" };
        return validOrders.Contains(sortOrder.ToLower());
    }
}