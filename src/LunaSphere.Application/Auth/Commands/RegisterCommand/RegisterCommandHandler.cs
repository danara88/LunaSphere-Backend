using MediatR;
using ErrorOr;

using LunaSphere.Application.Auth.DTOs;
using LunaSphere.Application.Auth.Interfaces;
using LunaSphere.Application.Common.Interfaces;
using LunaSphere.Domain.Users;
using System.Security.Cryptography;
using AutoMapper;
using LunaSphere.Application.Users.DTOs;
using LunaSphere.Domain.Exceptions;
using LunaSphere.Api.ApiMessagesConstants;

namespace LunaSphere.Application.Auth.Commands.RegisterCommand;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtFactory _jwtFactory;
    private readonly IMapper _mapper;

    public RegisterCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IJwtFactory jwtFactory,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtFactory = jwtFactory;
        _mapper = mapper;
    }

    public async Task<ErrorOr<AuthDTO>> Handle(RegisterCommand request, CancellationToken cancellationToken)
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

        string token = _jwtFactory.GenerateJwtToken(user);
        UserDTO userDTO = _mapper.Map<UserDTO>(user);

        await _unitOfWork.UserRepository.AddAsync(user);

        try
        {
            await _unitOfWork.SaveChangesAsync();
            var authDTO = new AuthDTO(AccessToken: token, UserDetails: userDTO);
            return authDTO;
        }
        catch
        {
            throw new InternalServerException(ApiMessagesConstants.Global.InternalServerError);
        }

    }

     private string CreateRandomeToken() => Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
    
}