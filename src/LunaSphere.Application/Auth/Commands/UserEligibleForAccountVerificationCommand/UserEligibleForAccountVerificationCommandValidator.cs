using FluentValidation;

namespace LunaSphere.Application.Auth.Commands.UserEligibleForAccountVerification;

/// <summary>
/// Validates the input data for UserEligibleForAccountVerificationCommand
/// </summary>
public class UserEligibleForAccountVerificationCommandValidator : AbstractValidator<UserEligibleForAccountVerificationCommand>
{
    public UserEligibleForAccountVerificationCommandValidator()
    {
        RuleFor(x => x.userEligibleForVerificationDTO.EncrytedVerificationToken)
            .NotEmpty()
            .OverridePropertyName("EncryptedVerificationToken");
    }
}