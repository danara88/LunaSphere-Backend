using ErrorOr;
using MediatR;

using LunaSphere.Application.Auth.DTOs;

namespace LunaSphere.Application.Auth.Commands.RegisterCommand;

public record RegisterCommand(RegisterUserDTO registerUserDTO) : IRequest<ErrorOr<AuthDTO>> {}