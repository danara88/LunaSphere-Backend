using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Net;

using LunaSphere.Api.Controllers;
using LunaSphere.Api.Responses;
using LunaSphere.Application.Auth.Commands.RegisterCommand;
using LunaSphere.Application.Auth.DTOs;
using LunaSphere.Application.Auth.Commands.LoginCommand;
using LunaSphere.Application.Auth.Commands.GoogleSignInCommand;

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
    public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerUserDTO)
    {
        var command = new RegisterCommand(registerUserDTO);
        var result = await _mediator.Send(command);

        Func<AuthDTO, IActionResult> response = (authDTO) => 
            Ok(new ApiResponse<AuthDTO>(authDTO, HttpStatusCode.Created));
        
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
}