using ErrorOr;
using MediatR;
using AutoMapper;

using LunaSphere.Application.Auth.DTOs;
using LunaSphere.Application.Common.Interfaces;
using LunaSphere.Application.Users.DTOs;
using LunaSphere.Application.Auth.Interfaces;
using LunaSphere.Domain.Users;
using LunaSphere.Domain.Exceptions;
using LunaSphere.Application.Constants;
using LunaSphere.Domain.Users.Events;

namespace LunaSphere.Application.Auth.Commands.GoogleSignInCommand;

public class GoogleSignInCommandHandler : IRequestHandler<GoogleSignInCommand, ErrorOr<AuthDTO>>
{
    private readonly IGoogleAuthService _googleAuthService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IJwtFactory _jwtFactory;
    private readonly IRefreshTokenFactory _refreshTokenFactory;

    public GoogleSignInCommandHandler(
        IGoogleAuthService googleAuthService,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IJwtFactory jwtFactory,
        IRefreshTokenFactory refreshTokenFactory)
    {
        _googleAuthService = googleAuthService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _jwtFactory = jwtFactory;
        _refreshTokenFactory = refreshTokenFactory;
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
            user = new User
            {
                FirstName = googleResp.Value.Name,
                LastName = googleResp.Value.FamilyName,
                Email = googleResp.Value.Email,
                IsGoogle = true,
                VerifiedAt = DateTime.UtcNow,
            };

            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.CommitChangesAsync();
        }
    
        user.LastLogin = DateTime.UtcNow;
        user._domainEvents.Add(new UserSignedInGoogleEvent(user.Id));

        try
        {
            await _unitOfWork.CommitChangesAsync();

            var token = _jwtFactory.GenerateJwtToken(user);
            var userDTO = _mapper.Map<UserDTO>(user);

            var authDTO = new AuthDTO
            (
                AccessToken: token,
                RefreshToken: "xyz",
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