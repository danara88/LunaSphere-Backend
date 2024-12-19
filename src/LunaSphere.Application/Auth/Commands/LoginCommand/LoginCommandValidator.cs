using FluentValidation;

namespace LunaSphere.Application.Auth.Commands.LoginCommand;

/// <summary>
/// Validates the input data to login a user
/// </summary>
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.loginUserDTO.Email)
            .NotEmpty()
            .EmailAddress()
            .OverridePropertyName("Email");

        RuleFor(x => x.loginUserDTO.Password)
            .NotEmpty()
            .OverridePropertyName("Password");
    }
}