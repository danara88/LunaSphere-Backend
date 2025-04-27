using ErrorOr;
using MediatR;
using AutoMapper;

using LunaSphere.Application.Auth.DTOs;
using LunaSphere.Application.Common.Interfaces;
using LunaSphere.Domain.Users;
using LunaSphere.Application.Users.DTOs;
using LunaSphere.Application.Auth.Interfaces;
using LunaSphere.Domain.RefreshTokens;
using LunaSphere.Domain.Exceptions;
using LunaSphere.Application.Constants;

namespace LunaSphere.Application.Auth.Commands.LoginCommand;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ErrorOr<AuthDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IJwtFactory _jwtFactory;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRefreshTokenFactory _refreshTokenFactory;

    public LoginCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IJwtFactory jwtFactory,
        IPasswordHasher passwordHasher,
        IRefreshTokenFactory refreshTokenFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _jwtFactory = jwtFactory;
        _passwordHasher = passwordHasher;
        _refreshTokenFactory = refreshTokenFactory;
    }

    public async Task<ErrorOr<AuthDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetByEmailAsync(request.loginUserDTO.Email);
        if (user is null) 
        {
            return AuthErrors.InvalidCredentials;
        }

        var isValidPassword = _passwordHasher.IsCorrectPassword(request.loginUserDTO.Password, user.PasswordHash);

        if (!isValidPassword) 
        {
            return AuthErrors.InvalidCredentials;
        }

        var userDTO = _mapper.Map<UserDTO>(user);
        var token =  _jwtFactory.GenerateJwtToken(user);

        var refreshToken = await _unitOfWork.RefreshTokenRepository.GetByUserIdAsync(user.Id);

        if (refreshToken is not null) 
        {
            refreshToken.Token = _refreshTokenFactory.GenerateRefreshToken();
            refreshToken.ExpiresAt = DateTime.UtcNow.AddMinutes(7);
        }
        else
        {
            refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = _refreshTokenFactory.GenerateRefreshToken(),
                ExpiresAt = DateTime.UtcNow.AddDays(1)
            };

            await _unitOfWork.RefreshTokenRepository.AddAsync(refreshToken);
        }

        try
        {
            await _unitOfWork.CommitChangesAsync();
            var authDTO = new AuthDTO 
            (
                AccessToken: token,
                RefreshToken: refreshToken.Token,
                UserDetails: userDTO
            );

            return authDTO;
        }
        catch
        {
            throw new InternalServerException(ApiMessagesConstants.Global.InternalServerError);
        }
    }
}