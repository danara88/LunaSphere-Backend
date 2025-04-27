using ErrorOr;
using MediatR;
using AutoMapper;

using LunaSphere.Application.Common.Interfaces;
using LunaSphere.Application.Constants;
using LunaSphere.Domain.Users;
using LunaSphere.Application.Auth.DTOs;
using LunaSphere.Application.Common.Helpers;
using LunaSphere.Domain.Exceptions;
using LunaSphere.Application.Auth.Interfaces;
using LunaSphere.Application.Users.DTOs;
using LunaSphere.Domain.Users.Events;

namespace LunaSphere.Application.Auth.Commands.VerifyVerificationCodeCommand;

public class VerifyVerificationCodeCommandHandler : IRequestHandler<VerifyVerificationCodeCommand, ErrorOr<AuthDTO>>
{
    private readonly ISecurityHelper _securityHelper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IJwtFactory _jwtFactory;
    private readonly IRefreshTokenFactory _refreshTokenFactory;

    public VerifyVerificationCodeCommandHandler(IUnitOfWork unitOfWork, ISecurityHelper securityHelper, IMapper mapper, IJwtFactory jwtFactory, IRefreshTokenFactory refreshTokenFactory)
    {
        _unitOfWork = unitOfWork;
        _securityHelper = securityHelper;
        _mapper = mapper;
        _jwtFactory = jwtFactory;
        _refreshTokenFactory = refreshTokenFactory;
    }

    public async Task<ErrorOr<AuthDTO>> Handle(VerifyVerificationCodeCommand request, CancellationToken cancellationToken)
    {
        string verificationToken;

        try
        {
            verificationToken = _securityHelper.DecryptString(request.verifyVerificationCodeDTO.userEligibleForVerification.EncrytedVerificationToken);
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

        if(userDB.VerifiedAt is not null)
        {
            return AuthErrors.UserAccountAlreadyVerified;
        }

        if(userDB.VerificationCodeExpires < DateTime.UtcNow)
        {
            return AuthErrors.VerificationCodeExpired;
        }

        if(userDB.VerificationCode != request.verifyVerificationCodeDTO.VerificationCode)
        {
            return AuthErrors.InvalidVerificationCode;
        }

        userDB.VerifiedAt = DateTime.UtcNow;
        userDB.LastLogin = DateTime.UtcNow;
        userDB.VerificationCodeExpires = null;
        userDB.VerificationCode = null;
        userDB.VerificationToken = null;
        userDB.VerificationTokenExpires = null;

        string refreshToken = _refreshTokenFactory.GenerateRefreshToken();

        userDB._domainEvents.Add(new UserAccountVerifiedEvent(userDB.Id, refreshToken));

        try 
        {
            await _unitOfWork.CommitChangesAsync();
            return new AuthDTO
            (
                UserDetails: _mapper.Map<UserDTO>(userDB),
                AccessToken: _jwtFactory.GenerateJwtToken(userDB),
                RefreshToken: refreshToken
            );
        }
        catch
        {
            throw new InternalServerException(ApiMessagesConstants.Auth.VerifyVerificationCodeFail);
        }
    }
}