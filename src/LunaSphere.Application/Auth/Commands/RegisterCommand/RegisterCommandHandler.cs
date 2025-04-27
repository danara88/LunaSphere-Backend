using MediatR;
using ErrorOr;
using System.Security.Cryptography;

using LunaSphere.Application.Common.Interfaces;
using LunaSphere.Domain.Users;
using LunaSphere.Domain.Exceptions;
using LunaSphere.Application.Constants;
using LunaSphere.Application.Auth.DTOs;
using LunaSphere.Domain.Users.Events;
using LunaSphere.Application.Common.Helpers;

namespace LunaSphere.Application.Auth.Commands.RegisterCommand;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<RegisterUserDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ISecurityHelper _securityHelper;

    public RegisterCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        ISecurityHelper securityHelper
    )
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _securityHelper = securityHelper;
    }

    public async Task<ErrorOr<RegisterUserDTO>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.UserRepository.ExistsByEmailAsync(request.createUserAccountDTO.Email))
        {
            return AuthErrors.UserAlreadyRegistered;
        }

        var hashPasswordResult = _passwordHasher.HashPassword(request.createUserAccountDTO.Password);

        if(hashPasswordResult.IsError)
        {
            return hashPasswordResult.Errors;
        }

        var user = new User
        {
            Email = request.createUserAccountDTO.Email,
            PasswordHash = hashPasswordResult.Value,
            VerificationCode = CreateRandomCode(),
            VerificationCodeExpires = DateTime.UtcNow.AddMinutes(1),
            VerificationToken = CreateRandomToken(),
            VerificationTokenExpires = DateTime.UtcNow.AddMinutes(5),
            LastVerificationEmailSent = DateTime.UtcNow
        };

        await _unitOfWork.UserRepository.AddAsync(user);

        try
        {
            await _unitOfWork.CommitChangesAsync();

            user._domainEvents.Add(new UserRegisteredEvent(user.Id));

            await _unitOfWork.CommitChangesAsync();
            
            return new RegisterUserDTO
            (
                VerificationToken: _securityHelper.EncryptString(user.VerificationToken),
                VerificationTokenExpires:  _securityHelper.EncryptString(user.VerificationTokenExpires.ToString()!)
            );
        }
        catch
        {
            throw new InternalServerException(ApiMessagesConstants.Auth.RegisterFail);
        }
    }

    private short CreateRandomCode() 
    {
        int min = 1000;
        int max = 9999;
        Random random = new Random();
        return (short)random.Next(min, max);
    }

    private string CreateRandomToken()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
    }
}