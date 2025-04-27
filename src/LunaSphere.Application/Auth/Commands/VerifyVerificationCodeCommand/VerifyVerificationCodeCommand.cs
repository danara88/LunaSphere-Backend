using ErrorOr;
using MediatR;

using LunaSphere.Application.Auth.DTOs;

namespace LunaSphere.Application.Auth.Commands.VerifyVerificationCodeCommand;

public record VerifyVerificationCodeCommand(VerifyVerificationCodeDTO verifyVerificationCodeDTO) : IRequest<ErrorOr<AuthDTO>> {}