using FluentValidation;

namespace LunaSphere.Application.Auth.Commands.ResendVerificationCodeCommand;

public class ResendVerificationCodeCommandValidator : AbstractValidator<ResendVerificationCodeCommand>
{
    public ResendVerificationCodeCommandValidator()
    {
       RuleFor(x => x.resendVerificationCodeDTO.EncrytedVerificationToken)
            .NotEmpty()
            .OverridePropertyName("EncryptedVerificationToken");
    }
}