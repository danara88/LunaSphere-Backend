using ErrorOr;
using MediatR;

using LunaSphere.Application.Auth.DTOs;

namespace LunaSphere.Application.Auth.Commands.RefreshTokenCommand;

public record RefreshTokenCommand(CreateRefreshTokenDTO createRefreshTokenDTO) : IRequest<ErrorOr<RefreshTokenDTO>> {}