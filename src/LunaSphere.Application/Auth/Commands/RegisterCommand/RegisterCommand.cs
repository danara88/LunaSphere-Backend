using ErrorOr;
using MediatR;

using LunaSphere.Application.Auth.DTOs;

namespace LunaSphere.Application.Auth.Commands.RegisterCommand;

public record RegisterCommand(CreateUserAccountDTO createUserAccountDTO) : IRequest<ErrorOr<RegisterUserDTO>> {}