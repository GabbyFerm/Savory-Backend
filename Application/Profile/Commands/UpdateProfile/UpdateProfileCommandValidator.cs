using FluentValidation;

namespace Application.Profile.Commands.UpdateProfile;

/// <summary>
/// Validator for UpdateProfileCommand
/// </summary>
public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        // UserName validation (if provided)
        RuleFor(x => x.UserName)
            .MinimumLength(3).WithMessage("Username must be at least 3 characters")
            .MaximumLength(50).WithMessage("Username must not exceed 50 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.UserName));

        // Email validation (if provided)
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email must be a valid email address")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        // AvatarColor validation (if provided)
        RuleFor(x => x.AvatarColor)
            .Matches(@"^#[0-9A-Fa-f]{6}$").WithMessage("Avatar color must be a valid hex color (e.g., #FF5733)")
            .When(x => !string.IsNullOrWhiteSpace(x.AvatarColor));
    }
}