using ErrorOr;
using MediatR;

using LunaSphere.Application.Auth.DTOs;
using LunaSphere.Application.Auth.Interfaces;
using LunaSphere.Application.Common.Interfaces;
using LunaSphere.Domain.Exceptions;
using LunaSphere.Domain.RefreshTokens;
using LunaSphere.Application.Constants;

namespace LunaSphere.Application.Auth.Commands.RefreshTokenCommand;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ErrorOr<RefreshTokenDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRefreshTokenFactory _refreshTokenFactory;
    private readonly IJwtFactory _jwtFactory;

    public RefreshTokenCommandHandler(
        IUnitOfWork unitOfWork,
        IRefreshTokenFactory refreshTokenFactory,
        IJwtFactory jwtFactory)
    {
        _unitOfWork = unitOfWork;
        _refreshTokenFactory = refreshTokenFactory;
        _jwtFactory = jwtFactory;
    }

    public async Task<ErrorOr<RefreshTokenDTO>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _unitOfWork.RefreshTokenRepository.GetByTokenAsync(request.createRefreshTokenDTO.RefreshToken);

        if (refreshToken is null || refreshToken.ExpiresAt < DateTime.UtcNow) 
        {
            return RefreshTokenErrors.RefreshTokenExpired;
        }

        var accessToken = _jwtFactory.GenerateJwtToken(refreshToken.User);

        refreshToken.Token = _refreshTokenFactory.GenerateRefreshToken();
        refreshToken.ExpiresAt = DateTime.UtcNow.AddDays(1);

        try
        {
            await _unitOfWork.CommitChangesAsync();
            
            var refreshTokenDTO = new RefreshTokenDTO
            (
                AccessToken: accessToken,
                RefreshToken: refreshToken.Token
            );

            return refreshTokenDTO;
        }
        catch
        {
            throw new InternalServerException(ApiMessagesConstants.Global.InternalServerError);
        }
    }
}