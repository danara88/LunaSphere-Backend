using MediatR;

using LunaSphere.Application.Common.Interfaces;
using LunaSphere.Domain.Users.Events;
using LunaSphere.Domain.Exceptions;

namespace LunaSphere.Application.Users.Events;

public class UserRegisteredEventHandler : INotificationHandler<UserRegisteredEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public UserRegisteredEventHandler(IUnitOfWork unitOfWork, IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        var userDB = await _unitOfWork.UserRepository.GetByIdAsync(notification.userId);

        if(userDB is null) return;

        try
        {
            await _emailService.SendVerificationEmail(userDB);
        }
        catch
        {
            throw new InternalServerException();
        }
    }
}