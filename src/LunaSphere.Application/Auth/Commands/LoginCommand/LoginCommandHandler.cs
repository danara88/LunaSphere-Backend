using ErrorOr;
using MediatR;
using AutoMapper;

using LunaSphere.Application.Auth.DTOs;
using LunaSphere.Application.Common.Interfaces;
using LunaSphere.Domain.Users;
using LunaSphere.Application.Users.DTOs;
using LunaSphere.Application.Auth.Interfaces;

namespace LunaSphere.Application.Auth.Commands.LoginCommand;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ErrorOr<AuthDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IJwtFactory _jwtFactory;
    private readonly IPasswordHasher _passwordHasher;

    public LoginCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IJwtFactory jwtFactory,
        IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _jwtFactory = jwtFactory;
        _passwordHasher = passwordHasher;
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

        var authDTO = new AuthDTO 
        (
            AccessToken: token,
            UserDetails: userDTO
        );

        return authDTO;
    }
}