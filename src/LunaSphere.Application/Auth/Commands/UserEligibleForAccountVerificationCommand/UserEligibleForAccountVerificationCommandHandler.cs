using ErrorOr;
using MediatR;

using LunaSphere.Application.Common.Helpers;
using LunaSphere.Application.Common.Interfaces;
using LunaSphere.Domain.Users;
using LunaSphere.Application.Constants;
using LunaSphere.Domain.Exceptions;

namespace LunaSphere.Application.Auth.Commands.UserEligibleForAccountVerification;

public class UserEligibleForAccountVerificationCommandHandler : IRequestHandler<UserEligibleForAccountVerificationCommand, ErrorOr<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISecurityHelper _securityHelper;

    public UserEligibleForAccountVerificationCommandHandler(IUnitOfWork unitOfWork, ISecurityHelper securityHelper)
    {
        _unitOfWork = unitOfWork;
        _securityHelper = securityHelper;
    }

    public async Task<ErrorOr<string>> Handle(UserEligibleForAccountVerificationCommand request, CancellationToken cancellationToken)
    {
        string verificationToken;

        try
        {
            verificationToken = _securityHelper.DecryptString(request.userEligibleForVerificationDTO.EncrytedVerificationToken);
        }
        catch
        {
            throw new InternalServerException(ApiMessagesConstants.Global.InternalServerError);
        }

        var userDB = await _unitOfWork.UserRepository.GetByVerificationTokenAsync(verificationToken);

        if(userDB is null || userDB.VerificationTokenExpires < DateTime.UtcNow || userDB.VerifiedAt is not null)
        {
            return AuthErrors.UserForbiddenAction;
        }

        return ApiMessagesConstants.Auth.UserAccountEligibleForVerification;
    }
};