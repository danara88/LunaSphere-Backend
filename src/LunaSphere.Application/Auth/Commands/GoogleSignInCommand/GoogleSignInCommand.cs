using ErrorOr;
using MediatR;

using LunaSphere.Application.Auth.DTOs;

namespace LunaSphere.Application.Auth.Commands.GoogleSignInCommand;

public record GoogleSignInCommand(GoogleSignInDTO googleSignInDTO) : IRequest<ErrorOr<AuthDTO>> {}