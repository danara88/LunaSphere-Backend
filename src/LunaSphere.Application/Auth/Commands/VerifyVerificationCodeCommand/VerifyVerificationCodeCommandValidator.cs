using FluentValidation;

namespace LunaSphere.Application.Auth.Commands.VerifyVerificationCodeCommand;

/// <summary>
/// Validates the input data for VerifyVerificationCodeCommandValidator
/// </summary>
public class VerifyVerificationCodeCommandValidator : AbstractValidator<VerifyVerificationCodeCommand>
{
    public VerifyVerificationCodeCommandValidator()
    {
        RuleFor(x => x.verifyVerificationCodeDTO.VerificationCode)
            .NotEmpty()
            .OverridePropertyName("VerificationCode");

        RuleFor(x => x.verifyVerificationCodeDTO.userEligibleForVerification.EncrytedVerificationToken)
        .NotEmpty()
        .OverridePropertyName("EncrytedVerificationToken");
    }
}