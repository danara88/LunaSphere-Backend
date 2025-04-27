using MediatR;

using LunaSphere.Application.Common.Interfaces;
using LunaSphere.Domain.Users.Events;
using LunaSphere.Domain.RefreshTokens;
using LunaSphere.Application.Auth.Interfaces;

namespace LunaSphere.Application.RefreshTokens.Events;

public class UserSignedInGoogleEventHandler : INotificationHandler<UserSignedInGoogleEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRefreshTokenFactory _refreshTokenFactory;

    public UserSignedInGoogleEventHandler(IUnitOfWork unitOfWork,  IRefreshTokenFactory refreshTokenFactory)
    {
        _unitOfWork = unitOfWork;
        _refreshTokenFactory = refreshTokenFactory;
    }

    public async Task Handle(UserSignedInGoogleEvent notification, CancellationToken cancellationToken)
    {

        var refreshTokenDB = await _unitOfWork.RefreshTokenRepository.GetByUserIdAsync(notification.userId);

        if(refreshTokenDB is null) 
        {
            var refreshToken = new RefreshToken
            {
                UserId = notification.userId,
                Token = _refreshTokenFactory.GenerateRefreshToken(),
                ExpiresAt = DateTime.UtcNow.AddDays(1)
            };

            await _unitOfWork.RefreshTokenRepository.AddAsync(refreshToken);
        }
        else
        {
            refreshTokenDB.ExpiresAt = DateTime.UtcNow.AddMinutes(7);
            refreshTokenDB.Token = _refreshTokenFactory.GenerateRefreshToken();
        }
        
        await _unitOfWork.CommitChangesAsync();
    }
}