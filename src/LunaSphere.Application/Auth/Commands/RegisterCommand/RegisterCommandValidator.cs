using FluentValidation;

namespace LunaSphere.Application.Auth.Commands.RegisterCommand;

/// <summary>
/// Validates the input data to register a user
/// </summary>
public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.createUserAccountDTO.Email)
            .NotEmpty()
            .EmailAddress()
            .OverridePropertyName("Email");

        RuleFor(x => x.createUserAccountDTO.Password)
            .NotEmpty()
            .Matches(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")
            .WithMessage("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character (#?!@$%^&*-).")
            .OverridePropertyName("Password");
    }
}