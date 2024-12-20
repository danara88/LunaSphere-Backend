using FluentValidation;

namespace LunaSphere.Application.Auth.Commands.GoogleSignInCommand;

/// <summary>
/// Validates the input data to sign in user with google
/// </summary>
public class GoogleSignInCommandValidator : AbstractValidator<GoogleSignInCommand>
{
    public GoogleSignInCommandValidator()
    {
        RuleFor(x => x.googleSignInDTO.Token)
            .NotEmpty()
            .OverridePropertyName("Token");
    }
}