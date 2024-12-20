using ErrorOr;
using MediatR;
using AutoMapper;

using LunaSphere.Application.Auth.DTOs;
using LunaSphere.Application.Common.Interfaces;
using LunaSphere.Application.Users.DTOs;
using LunaSphere.Application.Auth.Interfaces;
using LunaSphere.Domain.Users;
using LunaSphere.Domain.Exceptions;
using LunaSphere.Api.ApiMessagesConstants;

namespace LunaSphere.Application.Auth.Commands.GoogleSignInCommand;

public class GoogleSignInCommandHandler : IRequestHandler<GoogleSignInCommand, ErrorOr<AuthDTO>>
{
    private readonly IGoogleAuthService _googleAuthService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IJwtFactory _jwtFactory;

    public GoogleSignInCommandHandler(
        IGoogleAuthService googleAuthService, 
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        IJwtFactory jwtFactory)
    {
        _googleAuthService = googleAuthService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _jwtFactory = jwtFactory;
    }

    public async Task<ErrorOr<AuthDTO>> Handle(GoogleSignInCommand request, CancellationToken cancellationToken)
    {
        ErrorOr<GoogleAuthRespDTO> googleResp = await _googleAuthService.VerifyGoogleTokenAsync(request.googleSignInDTO.Token);

        if (googleResp.IsError)
        {
            return googleResp.Errors;
        }

        var user = await _unitOfWork.UserRepository.GetByEmailAsync(googleResp.Value.Email);

        if (user is null)
        {
            // If user does not exist in the database, create new account
            user = new User
            {
                FirstName = googleResp.Value.Name,
                LastName = googleResp.Value.FamilyName,
                Email = googleResp.Value.Email,
                LastLogin = DateTime.UtcNow,
                IsGoogle = true,
                VerifiedAt = DateTime.UtcNow
            };

            await _unitOfWork.UserRepository.AddAsync(user);
        }

        var token = _jwtFactory.GenerateJwtToken(user);
        var userDTO = _mapper.Map<UserDTO>(user);

        try
        {
            if(_unitOfWork.HasChanges())
            {
                await _unitOfWork.SaveChangesAsync();
            }
            
            var authDTO = new AuthDTO(
                AccessToken: token,
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