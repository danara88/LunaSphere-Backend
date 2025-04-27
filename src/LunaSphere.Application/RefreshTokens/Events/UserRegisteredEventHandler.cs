using MediatR;

using LunaSphere.Application.Common.Interfaces;
using LunaSphere.Domain.Users.Events;
using LunaSphere.Domain.RefreshTokens;

namespace LunaSphere.Application.RefreshTokens.Events;

public class UserAccountVerifiedEventtHandler : INotificationHandler<UserAccountVerifiedEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public UserAccountVerifiedEventtHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UserAccountVerifiedEvent notification, CancellationToken cancellationToken)
    {
        var refreshToken = new RefreshToken
        {
            UserId =  notification.userId,
            Token = notification.refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(1)
        };
        
        await _unitOfWork.RefreshTokenRepository.AddAsync(refreshToken);
        await _unitOfWork.CommitChangesAsync();
    }
}