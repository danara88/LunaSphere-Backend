using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Net;

using LunaSphere.Api.Controllers;
using LunaSphere.Api.Responses;
using LunaSphere.Application.Auth.Commands.RegisterCommand;
using LunaSphere.Application.Auth.DTOs;
using LunaSphere.Application.Auth.Commands.LoginCommand;
using LunaSphere.Application.Auth.Commands.GoogleSignInCommand;
using LunaSphere.Application.Auth.Commands.RefreshTokenCommand;
using LunaSphere.Application.Auth.Commands.UserEligibleForAccountVerification;
using LunaSphere.Application.Auth.Commands.VerifyVerificationCodeCommand;
using LunaSphere.Application.Auth.Commands.ResendVerificationCodeCommand;

namespace LunaSphere.Controllers;

public class AuthController : ApiController
{
    private readonly ISender _mediator;

    public AuthController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("~/api/v1/auth/register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] CreateUserAccountDTO createUserAccountDTO)
    {
        var command = new RegisterCommand(createUserAccountDTO);
        var result = await _mediator.Send(command);

        Func<RegisterUserDTO, IActionResult> response = (registerUserDTO) => 
            Ok(new ApiResponse<RegisterUserDTO>(registerUserDTO, HttpStatusCode.Created));
        
        return result.Match(
            response, 
            Problem
        );
    }

    [HttpPost("~/api/v1/auth/login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUserDTO)
    {
        var command = new LoginCommand(loginUserDTO);
        var result = await _mediator.Send(command);

        Func<AuthDTO, IActionResult> response = (authDTO) => 
            Ok(new ApiResponse<AuthDTO>(authDTO));
        
        return result.Match(
            response, 
            Problem
        );
    }

    [HttpPost("~/api/v1/auth/google-signin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GoogleSignIn([FromBody] GoogleSignInDTO googleSignInDTO)
    {
        var command = new GoogleSignInCommand(googleSignInDTO);
        var result = await _mediator.Send(command);

        Func<AuthDTO, IActionResult> response = (authDTO) => 
            Ok(new ApiResponse<AuthDTO>(authDTO));
        
        return result.Match(
            response, 
            Problem
        );
    }

    [HttpPost("~/api/v1/auth/refresh-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RefreshToken([FromBody] CreateRefreshTokenDTO createRefreshTokenDTO)
    {
        var command = new RefreshTokenCommand(createRefreshTokenDTO);
        var result = await _mediator.Send(command);

        Func<RefreshTokenDTO, IActionResult> response = (refreshTokenDTO) => 
            Ok(new ApiResponse<RefreshTokenDTO>(refreshTokenDTO));
        
        return result.Match(
            response, 
            Problem
        );
    }

    [HttpPost("~/api/v1/auth/resend-verification-code")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ResendVerificationCode([FromBody] ResendVerificationCodeDTO resendVerificationCodeDTO)
    {
        var command = new ResendVerificationCodeCommand(resendVerificationCodeDTO);
        var result = await _mediator.Send(command);

        Func<string, IActionResult> response = (message) => 
            Ok(new ApiMessageResponse(message));
        
        return result.Match(
            response, 
            Problem
        );
    }

    [HttpPost("~/api/v1/auth/user-eligible-for-verification")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UserEligibleForVerification([FromQuery] UserEligibleForVerificationDTO userEligibleForVerificationDTO)
    {
        var command = new UserEligibleForAccountVerificationCommand(userEligibleForVerificationDTO);
        var result = await _mediator.Send(command);

        Func<string, IActionResult> response = (message) => 
            Ok(new ApiMessageResponse(message));
        
        return result.Match(
            response, 
            Problem
        );
    }

    [HttpPost("~/api/v1/auth/verify-verification-code")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> VerifyVerificationCode([FromBody] VerifyVerificationCodeDTO verifyVerificationCodeDTO)
    {
        var command = new VerifyVerificationCodeCommand(verifyVerificationCodeDTO);
        var result = await _mediator.Send(command);

        Func<AuthDTO, IActionResult> response = (authDTO) => 
            Ok(new ApiResponse<AuthDTO>(authDTO));
        
        return result.Match(
            response, 
            Problem
        );
    }
}