using MediatR;

using LunaSphere.Application.Common.Interfaces;
using LunaSphere.Domain.Users.Events;
using LunaSphere.Domain.RefreshTokens;

namespace LunaSphere.Application.RefreshTokens.Events;

public class UserSignedInGoogleEventHandler : INotificationHandler<UserSignedInGoogleEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public UserSignedInGoogleEventHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UserSignedInGoogleEvent notification, CancellationToken cancellationToken)
    {
        var refreshTokenDB = await _unitOfWork.RefreshTokenRepository.GetByUserIdAsync(notification.userId);

        if(refreshTokenDB is null) 
        {
            var refreshToken = new RefreshToken
            {
                UserId = notification.userId,
                Token = notification.refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(1)
            };

            await _unitOfWork.RefreshTokenRepository.AddAsync(refreshToken);
        }
        else
        {
            refreshTokenDB.ExpiresAt = DateTime.UtcNow.AddMinutes(7);
            refreshTokenDB.Token = notification.refreshToken;
        }
        
        await _unitOfWork.CommitChangesAsync();
    }
}