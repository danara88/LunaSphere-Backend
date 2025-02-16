using FluentValidation;

namespace LunaSphere.Application.Auth.Commands.RefreshTokenCommand;

/// <summary>
/// Validates the input data for refresh token command
/// </summary>
public class RefreshTokenCommandValdiator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValdiator()
    {
        RuleFor(x => x.createRefreshTokenDTO.RefreshToken)
            .NotEmpty()
            .OverridePropertyName("RefreshToken");
    }
}