using ErrorOr;
using MediatR;

using LunaSphere.Application.Auth.DTOs;

namespace LunaSphere.Application.Auth.Commands.ResendVerificationCodeCommand;

public record ResendVerificationCodeCommand(ResendVerificationCodeDTO resendVerificationCodeDTO) : IRequest<ErrorOr<string>> {}