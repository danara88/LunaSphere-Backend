using ErrorOr;
using MediatR;

using LunaSphere.Application.Auth.DTOs;

namespace LunaSphere.Application.Auth.Commands.LoginCommand;

public record LoginCommand(LoginUserDTO loginUserDTO) : IRequest<ErrorOr<AuthDTO>> {}