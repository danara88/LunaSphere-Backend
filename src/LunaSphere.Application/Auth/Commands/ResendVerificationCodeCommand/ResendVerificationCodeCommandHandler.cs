using ErrorOr;
using MediatR;

using LunaSphere.Application.Common.Interfaces;
using LunaSphere.Application.Constants;
using LunaSphere.Domain.Exceptions;
using LunaSphere.Domain.Users;
using LunaSphere.Application.Common.Helpers;
using LunaSphere.Domain.Users.Events;

namespace LunaSphere.Application.Auth.Commands.ResendVerificationCodeCommand;

public class ResendVerificationCodeCommandHandler : IRequestHandler<ResendVerificationCodeCommand, ErrorOr<string>>
{
    private readonly IUnitOfWork _unitOfWork;
     private readonly ISecurityHelper _securityHelper;

    public ResendVerificationCodeCommandHandler(IUnitOfWork unitOfWork, ISecurityHelper securityHelper)
    {
        _unitOfWork = unitOfWork;
        _securityHelper = securityHelper;
    }

    public async Task<ErrorOr<string>> Handle(ResendVerificationCodeCommand request, CancellationToken cancellationToken)
    {
        string verificationToken;

        try
        {
            verificationToken = _securityHelper.DecryptString(request.resendVerificationCodeDTO.EncrytedVerificationToken);
        }
        catch
        {
            throw new InternalServerException(ApiMessagesConstants.Global.InternalServerError);
        }

        var userDB = await _unitOfWork.UserRepository.GetByVerificationTokenAsync(verificationToken);

        if(userDB is null || userDB.VerificationTokenExpires < DateTime.UtcNow)
        {
            return AuthErrors.InvalidVerificationToken;
        }

        if (userDB.VerifiedAt is not null)
        {
            return AuthErrors.UserAccountAlreadyVerified;
        }
        
        TimeSpan? timeLapse = DateTime.UtcNow - userDB.LastVerificationEmailSent;

        if(timeLapse is not null && timeLapse.Value.Minutes < 1)
        {
            return AuthErrors.VerificationCodeNotExpiredYet;
        }

        userDB.LastVerificationEmailSent = DateTime.UtcNow;
        userDB.VerificationCode = CreateRandomCode();
        userDB.VerificationCodeExpires = DateTime.UtcNow.AddMinutes(1);
        userDB._domainEvents.Add(new UserRegisteredEvent(userDB.Id));

        try 
        {
            await _unitOfWork.CommitChangesAsync();
            return ApiMessagesConstants.Auth.GenerateVerificationCodeSuccess;
        } 
        catch
        {
            throw new InternalServerException(ApiMessagesConstants.Auth.GenerateVerificationCodeFail);
        }
    }

    private short CreateRandomCode() 
    {
        int min = 1000;
        int max = 9999;
        Random random = new Random();
        return (short)random.Next(min, max);
    }
}