using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using FluentAssertions;
using NSubstitute;
using MediatR;

using LunaSphere.Api.Responses;
using LunaSphere.Application.Auth.Commands.RegisterCommand;
using LunaSphere.Application.Auth.DTOs;
using LunaSphere.Application.Users.DTOs;
using LunaSphere.Controllers;
using LunaSphere.Domain.Users;
using LunaSphere.Application.Auth.Commands.LoginCommand;
using LunaSphere.Application.Auth.Commands.GoogleSignInCommand;
using LunaSphere.Application.Auth.Commands.RefreshTokenCommand;
using LunaSphere.Domain.Users.Enums;
using LunaSphere.Application.Auth.Commands.UserEligibleForAccountVerification;
using LunaSphere.Application.Constants;
using LunaSphere.Application.Auth.Commands.VerifyVerificationCodeCommand;
using LunaSphere.Application.Auth.Commands.ResendVerificationCodeCommand;

namespace LunaSphere.Api.Tests.Unit.Controllers;

public class AuthControllerTests
{
    private readonly AuthController _sut;
    private readonly ISender _mediator = Substitute.For<ISender>();

    public AuthControllerTests()
    {
        _sut = new AuthController(_mediator);
    }

    [Fact]
    public async Task Register_ShouldReturnOkCreatedAndObject_WhenAllInputsAreValid()
    {
        // Arrange
        var mockRegisterDTO = new CreateUserAccountDTO(Email: "test@tests.com", Password: "Strong_Password!");
        var mockRegisterUserDTO = new RegisterUserDTO
        (
            VerificationToken: "XYZ",
            VerificationTokenExpires: "HJGJ"
        );
        _mediator.Send(Arg.Any<RegisterCommand>()).Returns(mockRegisterUserDTO);

        // Act
        var result = (ObjectResult) await _sut.Register(mockRegisterDTO);

        // Assert
        result.Value.Should().BeEquivalentTo(new ApiResponse<RegisterUserDTO>(mockRegisterUserDTO, HttpStatusCode.Created));
    }
    
    [Fact]
    public async Task Register_ShouldReturnConflictErrorWithExpectedMessage_WhenEmailAlreadyExists() {
        // Arrange
        var mockRegisterDTO = new CreateUserAccountDTO(Email: "test@tests.com", Password: "Strong_Password!");
        _mediator.Send(Arg.Any<RegisterCommand>()).Returns(AuthErrors.UserAlreadyRegistered);

        // Act
        var result = (ObjectResult) await _sut.Register(mockRegisterDTO);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status409Conflict);
        result.Value.Should().BeEquivalentTo(new ProblemDetails 
        {
            Status =StatusCodes.Status409Conflict,
            Detail = AuthErrors.UserAlreadyRegistered.Description
        });
    }

    [Fact]
    public async Task Register_ShouldReturnValidationErrorWithExpectedMessage_WhenPasswordIsTooWeak() {
        // Arrange
        var mockRegisterDTO = new CreateUserAccountDTO(Email: "test@tests.com", Password: "Weak_Password!");
        _mediator.Send(Arg.Any<RegisterCommand>()).Returns(AuthErrors.WeakPassword);

        // Act
        var result = (ObjectResult) await _sut.Register(mockRegisterDTO);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Value.Should().BeEquivalentTo(new ProblemDetails 
        {
            Status =StatusCodes.Status400BadRequest,
            Detail = AuthErrors.WeakPassword.Description
        });
    }

    [Fact]
    public async Task Login_ShouldReturnOkObjectResult_WhenCredentialsAreValid()
    {
        // Arrange
         var mockLoginUser = new LoginUserDTO(Email: "test@tests.com", Password: "Strong_Password!");
         var mockAuthDTO = new AuthDTO
        (
            AccessToken: "xyztokenxyz",
            RefreshToken: "xyzrefreshtokenxyz",
            UserDetails: new UserDTO
            (
                Id: 1,
                FirstName: null,
                LastName: null,
                Role: RoleType.standard,
                Email: mockLoginUser.Email,
                IsGoogle: false,
                VerifiedAt: null,
                LastLogin: DateTime.UtcNow
            )
        );
         _mediator.Send(Arg.Any<LoginCommand>()).Returns(mockAuthDTO);

        // Act
        var result = (ObjectResult) await _sut.Login(mockLoginUser);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Value.Should().BeEquivalentTo(new ApiResponse<AuthDTO>(mockAuthDTO));
    }

    [Fact]
    public async Task Login_ShouldReturnUnAuthorizedWithExpectedMessage_WhenCredentialsAreInvalid()
    {
        // Arrange
        var mockLoginUserDTO = new LoginUserDTO(Email: "test@tests.com", Password: "123");
        _mediator.Send(Arg.Any<LoginCommand>()).Returns(AuthErrors.InvalidCredentials);

        // Act
        var result = (ObjectResult) await _sut.Login(mockLoginUserDTO);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        result.Value.Should().BeEquivalentTo(new ProblemDetails 
        {
            Status =StatusCodes.Status401Unauthorized,
            Detail = AuthErrors.InvalidCredentials.Description
        });
    }

    [Fact]
    public async Task GoogleSignIn_ShouldReturnOkObjectResult_WhenTokenIsValid()
    {
        // Arrange
        var mockGoogleSignInDTO = new GoogleSignInDTO(Token: "xyz123xyz");
        var mockAuthDTO = new AuthDTO
        (
            AccessToken: "xyztokenxyz",
            RefreshToken: "xyzrefreshtokenxyz",
            UserDetails: new UserDTO
            (
                Id: 1,
                FirstName: null,
                LastName: null,
                Role: RoleType.standard,
                Email: "test@test.com",
                VerifiedAt: null,
                IsGoogle: true,
                LastLogin: DateTime.UtcNow
            )
        );
         _mediator.Send(Arg.Any<GoogleSignInCommand>()).Returns(mockAuthDTO);

        // Act
        var result = (ObjectResult) await _sut.GoogleSignIn(mockGoogleSignInDTO);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Value.Should().BeEquivalentTo(new ApiResponse<AuthDTO>(mockAuthDTO));
    }

    [Fact]
    public async Task GoogleSignIn_ShouldReturnUnAuthorizedWithExpectedMessage_WhenTokenIsInvalid()
    {
        // Arrange
        var mockGoogleSignInDTO = new GoogleSignInDTO(Token: "xyinvalidtokenyz");
        _mediator.Send(Arg.Any<GoogleSignInCommand>()).Returns(AuthErrors.GoogleInvalidJwtToken);

        // Act
        var result = (ObjectResult) await _sut.GoogleSignIn(mockGoogleSignInDTO);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        result.Value.Should().BeEquivalentTo(new ProblemDetails 
        {
            Status =StatusCodes.Status401Unauthorized,
            Detail = AuthErrors.GoogleInvalidJwtToken.Description
        });
    }

    [Fact]
    public async Task GoogleSignIn_ShouldReturnFailureWithExpectedMessage_WhenGoogleSignInFails()
    {
        // Arrange
        var mockGoogleSignInDTO = new GoogleSignInDTO(Token: "xyinvalidtokenyz");
        _mediator.Send(Arg.Any<GoogleSignInCommand>()).Returns(AuthErrors.GoogleAuthFailure);

        // Act
        var result = (ObjectResult) await _sut.GoogleSignIn(mockGoogleSignInDTO);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        result.Value.Should().BeEquivalentTo(new ProblemDetails 
        {
            Status =StatusCodes.Status500InternalServerError,
            Detail = AuthErrors.GoogleAuthFailure.Description
        });
    }

    [Fact]
    public async Task RefreshToken_ShouldReturnOkObjectResult_WhenRefreshTokenIsValid()
    {
        // Arrange
        var mockCreateRefreshTokenDTO = new CreateRefreshTokenDTO(RefreshToken: "xyztokenxyz");
        var mockRefreshTokenDTO = new RefreshTokenDTO
        (
            AccessToken: "xyztokenxyz",
            RefreshToken: "xyznewrefreshtokenxyz"
        );
        _mediator.Send(Arg.Any<RefreshTokenCommand>()).Returns(mockRefreshTokenDTO);

        // Act
        var result = (ObjectResult) await _sut.RefreshToken(mockCreateRefreshTokenDTO);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Value.Should().BeEquivalentTo(new ApiResponse<RefreshTokenDTO>(mockRefreshTokenDTO));
    }

    [Fact]
    public async Task UserVerificationEligibility_ShouldReturnOkObjectResult_WhenUserIsEligible()
    {
        // Arrange
        var mockUserEligibleForVerificationDTO = new UserEligibleForVerificationDTO
        (
            EncrytedVerificationToken: "FakeEncryptedVerificatioTokenXyz"
        );
        _mediator.Send(Arg.Any<UserEligibleForAccountVerificationCommand>()).Returns(ApiMessagesConstants.Auth.UserAccountEligibleForVerification);

        // Act
        var result = (ObjectResult) await _sut.UserEligibleForVerification(mockUserEligibleForVerificationDTO);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Value.Should().BeEquivalentTo(new ApiMessageResponse(ApiMessagesConstants.Auth.UserAccountEligibleForVerification));
    }

    [Fact]
    public async Task UserVerificationEligibility_ShouldReturnUnForbbidenWithExpectedMessage_WhenUserINotEligible()
    {
        // Arrange
        var mockUserEligibleForVerificationDTO = new UserEligibleForVerificationDTO
        (
            EncrytedVerificationToken: "FakeEncryptedVerificatioTokenXyz"
        );
        _mediator.Send(Arg.Any<UserEligibleForAccountVerificationCommand>()).Returns(AuthErrors.UserForbiddenAction);

        // Act
        var result = (ObjectResult) await _sut.UserEligibleForVerification(mockUserEligibleForVerificationDTO);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        result.Value.Should().BeEquivalentTo(new ProblemDetails 
        {
            Status = StatusCodes.Status403Forbidden,
            Detail = AuthErrors.UserForbiddenAction.Description
        });
    }

    [Fact]
    public async Task VerifyVerificationCode_ShouldReturnOkObjectResult_WhenVerificationSucceed()
    {
        // Arrange
        var mockVerifyVerificationCodeDTO = new VerifyVerificationCodeDTO
        (
            VerificationCode: 0111,
            userEligibleForVerification: new UserEligibleForVerificationDTO(EncrytedVerificationToken: "XYZ")
              
        );
        var mockAuthDTO = new AuthDTO
        (
            AccessToken: "xyztokenxyz",
            RefreshToken: "xyzrefreshtokenxyz",
            UserDetails: new UserDTO
            (
                Id: 1,
                FirstName: null,
                LastName: null,
                Role: RoleType.standard,
                Email: "test@test.com",
                VerifiedAt: null,
                IsGoogle: true,
                LastLogin: DateTime.UtcNow
            )
        );
        _mediator.Send(Arg.Any<VerifyVerificationCodeCommand>()).Returns(mockAuthDTO);

        // Act
        var result = (ObjectResult) await _sut.VerifyVerificationCode(mockVerifyVerificationCodeDTO);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Value.Should().BeEquivalentTo(new ApiResponse<AuthDTO>(mockAuthDTO));
    }


    [Fact]
    public async Task VerifyVerificationCode_ShouldReturnValidationError_WhenVerificationFails()
    {
        // Arrange
        var mockVerifyVerificationCodeDTO = new VerifyVerificationCodeDTO
        (
            VerificationCode: 0111,
            userEligibleForVerification: new UserEligibleForVerificationDTO(EncrytedVerificationToken: "XYZ")
              
        );
    
        _mediator.Send(Arg.Any<VerifyVerificationCodeCommand>()).Returns(AuthErrors.InvalidVerificationToken);

        // Act
        var result = (ObjectResult) await _sut.VerifyVerificationCode(mockVerifyVerificationCodeDTO);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Value.Should().BeEquivalentTo(new ProblemDetails 
        {
            Status = StatusCodes.Status400BadRequest,
            Detail = AuthErrors.InvalidVerificationToken.Description
        });
    }

    [Fact]
    public async Task ResendVerificationCode_ShouldReturnOkObjectResult_WhenResendVerificationCodeSucceed()
    {
        // Arrange
        var mockResendVerificationCodeDTO = new ResendVerificationCodeDTO(EncrytedVerificationToken: "xyztokenxyz");
        _mediator.Send(Arg.Any<ResendVerificationCodeCommand>()).Returns(ApiMessagesConstants.Auth.GenerateVerificationCodeSuccess);

        // Act
        var result = (ObjectResult) await _sut.ResendVerificationCode(mockResendVerificationCodeDTO);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Value.Should().BeEquivalentTo(new ApiMessageResponse(ApiMessagesConstants.Auth.GenerateVerificationCodeSuccess));
    }

    [Fact]
    public async Task ResendVerificationCode_ShouldReturnErrors_WhenResendVerificationCodeFails()
    {
        // Arrange
        var mockResendVerificationCodeDTO = new ResendVerificationCodeDTO(EncrytedVerificationToken: "xyztokenxyz");
        _mediator.Send(Arg.Any<ResendVerificationCodeCommand>()).Returns(AuthErrors.UserAccountAlreadyVerified);

        // Act
        var result = (ObjectResult) await _sut.ResendVerificationCode(mockResendVerificationCodeDTO);

        // Assert
         result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Value.Should().BeEquivalentTo(new ProblemDetails 
        {
            Status = StatusCodes.Status400BadRequest,
            Detail = AuthErrors.UserAccountAlreadyVerified.Description
        });
    }
}