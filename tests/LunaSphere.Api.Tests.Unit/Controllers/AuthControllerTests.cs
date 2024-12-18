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
        var mockRegisterDTO = new RegisterUserDTO(Email: "test@tests.com", Password: "Strong_Password!");
        var mockAuthDTO = new AuthDTO
        (
            AccessToken: "xyztokenxyz",
            UserDetails: new UserDTO
            (
                FirstName: null,
                LastName: null,
                Email: mockRegisterDTO.Email,
                IsGoogle: false,
                LastLogin: DateTime.UtcNow
            )
        );
        _mediator.Send(Arg.Any<RegisterCommand>()).Returns(mockAuthDTO);

        // Act
        var result = (ObjectResult) await _sut.Register(mockRegisterDTO);

        // Assert
        result.Value.Should().BeEquivalentTo(new ApiResponse<AuthDTO>(mockAuthDTO, HttpStatusCode.Created));
    }
    

    [Fact]
    public async Task Register_ShouldReturnConflictErrorWithExpectedMessage_WhenEmailAlreadyExists() {
        // Arrange
        var mockRegisterDTO = new RegisterUserDTO(Email: "test@tests.com", Password: "Strong_Password!");
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
        var mockRegisterDTO = new RegisterUserDTO(Email: "test@tests.com", Password: "Weak_Password!");
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
}