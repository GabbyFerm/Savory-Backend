using FluentValidation;

namespace Application.TodoItems.Queries.GetTodoItemById;

public class GetTodoItemByIdQueryValidator : AbstractValidator<GetTodoItemByIdQuery>
{
    public GetTodoItemByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
    }
}