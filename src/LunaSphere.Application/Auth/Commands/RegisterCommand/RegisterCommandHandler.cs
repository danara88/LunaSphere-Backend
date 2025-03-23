using MediatR;
using ErrorOr;
using System.Security.Cryptography;
using AutoMapper;

using LunaSphere.Application.Auth.Interfaces;
using LunaSphere.Application.Common.Interfaces;
using LunaSphere.Domain.Users;
using LunaSphere.Domain.Exceptions;
using LunaSphere.Application.Constants;

namespace LunaSphere.Application.Auth.Commands.RegisterCommand;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtFactory _jwtFactory;
    private readonly IMapper _mapper;
    private readonly IRefreshTokenFactory _refreshTokenFactory;

    public RegisterCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IJwtFactory jwtFactory,
        IMapper mapper,
        IRefreshTokenFactory refreshTokenFactory)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtFactory = jwtFactory;
        _mapper = mapper;
        _refreshTokenFactory = refreshTokenFactory;
    }

    public async Task<ErrorOr<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.UserRepository.ExistsByEmailAsync(request.registerUserDTO.Email))
        {
            return AuthErrors.UserAlreadyRegistered;
        }

        var hashPasswordResult = _passwordHasher.HashPassword(request.registerUserDTO.Password);
        if(hashPasswordResult.IsError)
        {
            return hashPasswordResult.Errors;
        }

        var user = new User
        {
            Email = request.registerUserDTO.Email,
            PasswordHash = hashPasswordResult.Value,
            VerificationToken = CreateRandomeToken(),
            VerificationTokenExpires = DateTime.UtcNow.AddDays(2),
            LastLogin = DateTime.UtcNow
        };

        await _unitOfWork.UserRepository.AddAsync(user);

        try
        {
            await _unitOfWork.SaveChangesAsync();
            // var refreshToken = new RefreshToken
            // {
            //     UserId =  user.Id,
            //     Token = _refreshTokenFactory.GenerateRefreshToken(),
            //     ExperiesAt = DateTime.UtcNow.AddDays(1)
            // };

            // await _unitOfWork.RefreshTokenRepository.AddAsync(refreshToken);

            // await _unitOfWork.SaveChangesAsync();

            return ApiMessagesConstants.Auth.RegisterSuccess;
        }
        catch
        {
            throw new InternalServerException(ApiMessagesConstants.Global.InternalServerError);
        }

    }

     private string CreateRandomeToken() => Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
    
}