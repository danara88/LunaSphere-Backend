using ErrorOr;
using MediatR;

using LunaSphere.Application.Auth.DTOs;

namespace LunaSphere.Application.Auth.Commands.UserEligibleForAccountVerification;

public record UserEligibleForAccountVerificationCommand(UserEligibleForVerificationDTO userEligibleForVerificationDTO) : IRequest<ErrorOr<string>> {};