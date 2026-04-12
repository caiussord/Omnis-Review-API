using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using OmnisReview.Models;
using OmnisReview.Controllers;
using OmnisReview.Services.Interfaces;
using OmnisReview.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OmnisReview.Tests.Controllers;

[TestFixture]
public class AuthControllerTests
{
    private AuthController _authController = null!;
    private Mock<IAuthService> _mockAuthService = null!;

    [SetUp]
    public void SetUp()
    {
        _mockAuthService = new Mock<IAuthService>();
        _authController = new AuthController(_mockAuthService.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _mockAuthService?.Reset();
    }

    #region Login Tests

    [Test]
    public async Task Login_WithValidCredentials_ReturnsOkResult()
    {
        // Arrange
        var loginDto = new LoginDtoBuilder().Build();
        var authResponse = new AuthResponseDto
        {
            IsSuccess = true,
            Token = "jwt-token-123",
            Expiration = DateTime.UtcNow.AddHours(1),
            Message = "User logged in successfully"
        };

        _mockAuthService
            .Setup(x => x.LoginAsync(loginDto))
            .ReturnsAsync(authResponse);

        // Act
        var result = await _authController.Login(loginDto);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult?.Value, Is.Not.Null);
        _mockAuthService.Verify(x => x.LoginAsync(loginDto), Times.Once);
    }

    [Test]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorizedResult()
    {
        // Arrange
        var loginDto = new LoginDtoBuilder().Build();
        var authResponse = new AuthResponseDto
        {
            IsSuccess = false,
            Message = "Invalid login attempt"
        };

        _mockAuthService
            .Setup(x => x.LoginAsync(loginDto))
            .ReturnsAsync(authResponse);

        // Act
        var result = await _authController.Login(loginDto);

        // Assert
        Assert.That(result, Is.TypeOf<UnauthorizedObjectResult>());
        var unauthorizedResult = result as UnauthorizedObjectResult;
        Assert.That(unauthorizedResult?.StatusCode, Is.EqualTo(401));
    }

    #endregion

    #region Register Tests

    [Test]
    public async Task Register_WithValidData_ReturnsOkResult()
    {
        // Arrange
        var registerDto = new RegisterDtoBuilder().Build();
        var authResponse = new AuthResponseDto
        {
            IsSuccess = true,
            Message = "User created successfully"
        };

        _mockAuthService
            .Setup(x => x.RegisterAsync(registerDto))
            .ReturnsAsync(authResponse);

        // Act
        var result = await _authController.Register(registerDto);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        _mockAuthService.Verify(x => x.RegisterAsync(registerDto), Times.Once);
    }

    [Test]
    public async Task Register_WithExistingEmail_Returns500StatusCode()
    {
        // Arrange
        var registerDto = new RegisterDtoBuilder().Build();
        var authResponse = new AuthResponseDto
        {
            IsSuccess = false,
            Message = "Email already exists"
        };

        _mockAuthService
            .Setup(x => x.RegisterAsync(registerDto))
            .ReturnsAsync(authResponse);

        // Act
        var result = await _authController.Register(registerDto);

        // Assert
        Assert.That(result, Is.TypeOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
    }

    #endregion

    #region ForgotPassword Tests

    [Test]
    public async Task ForgotPassword_WithValidEmail_ReturnsOkResult()
    {
        // Arrange
        var forgotPasswordDto = new ForgotPasswordDto { UserNameOrEmail = "user@example.com" };
        var authResponse = new AuthResponseDto
        {
            IsSuccess = true,
            Message = "If the account exists, a reset link was sent."
        };

        _mockAuthService
            .Setup(x => x.ForgotPasswordAsync(forgotPasswordDto))
            .ReturnsAsync(authResponse);

        // Act
        var result = await _authController.ForgotPassword(forgotPasswordDto);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        _mockAuthService.Verify(x => x.ForgotPasswordAsync(forgotPasswordDto), Times.Once);
    }

    [Test]
    public async Task ForgotPassword_WithInvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var forgotPasswordDto = new ForgotPasswordDto { UserNameOrEmail = "" };
        var authResponse = new AuthResponseDto
        {
            IsSuccess = false,
            Message = "Invalid request"
        };

        _mockAuthService
            .Setup(x => x.ForgotPasswordAsync(forgotPasswordDto))
            .ReturnsAsync(authResponse);

        // Act
        var result = await _authController.ForgotPassword(forgotPasswordDto);

        // Assert
        Assert.That(result, Is.TypeOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
    }

    #endregion

    #region ResetPassword Tests

    [Test]
    public async Task ResetPassword_WithValidToken_ReturnsOkResult()
    {
        // Arrange
        var resetPasswordDto = new ResetPasswordDto
        {
            UserId = Guid.NewGuid(),
            Token = "reset-token",
            NewPassword = "NewPassword123!"
        };
        var authResponse = new AuthResponseDto
        {
            IsSuccess = true,
            Message = "Password reset successfully"
        };

        _mockAuthService
            .Setup(x => x.ResetPasswordAsync(resetPasswordDto))
            .ReturnsAsync(authResponse);

        // Act
        var result = await _authController.ResetPassword(resetPasswordDto);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        _mockAuthService.Verify(x => x.ResetPasswordAsync(resetPasswordDto), Times.Once);
    }

    [Test]
    public async Task ResetPassword_WithInvalidToken_ReturnsBadRequest()
    {
        // Arrange
        var resetPasswordDto = new ResetPasswordDto
        {
            UserId = Guid.NewGuid(),
            Token = "invalid-token",
            NewPassword = "NewPassword123!"
        };
        var authResponse = new AuthResponseDto
        {
            IsSuccess = false,
            Message = "Invalid token"
        };

        _mockAuthService
            .Setup(x => x.ResetPasswordAsync(resetPasswordDto))
            .ReturnsAsync(authResponse);

        // Act
        var result = await _authController.ResetPassword(resetPasswordDto);

        // Assert
        Assert.That(result, Is.TypeOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
    }

    #endregion

    #region ChangePassword Tests

    [Test]
    public async Task ChangePassword_WithValidData_ReturnsOkResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var changePasswordDto = new ChangePasswordDto
        {
            CurrentPassword = "OldPassword123!",
            NewPassword = "NewPassword123!"
        };
        var authResponse = new AuthResponseDto
        {
            IsSuccess = true,
            Message = "Password changed successfully"
        };

        _mockAuthService
            .Setup(x => x.ChangePasswordAsync(userId, changePasswordDto))
            .ReturnsAsync(authResponse);

        // Setup the controller's User claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        _authController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };

        // Act
        var result = await _authController.ChangePassword(changePasswordDto);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        _mockAuthService.Verify(x => x.ChangePasswordAsync(userId, changePasswordDto), Times.Once);
    }

    [Test]
    public async Task ChangePassword_WithoutAuthenticatedUser_ReturnsUnauthorized()
    {
        // Arrange
        var changePasswordDto = new ChangePasswordDto
        {
            CurrentPassword = "OldPassword123!",
            NewPassword = "NewPassword123!"
        };

        // Setup the controller with no user claim
        var identity = new ClaimsIdentity();
        var principal = new ClaimsPrincipal(identity);
        _authController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };

        // Act
        var result = await _authController.ChangePassword(changePasswordDto);

        // Assert
        Assert.That(result, Is.TypeOf<UnauthorizedObjectResult>());
    }

    [Test]
    public async Task ChangePassword_WithIncorrectPassword_ReturnsBadRequest()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var changePasswordDto = new ChangePasswordDto
        {
            CurrentPassword = "WrongPassword123!",
            NewPassword = "NewPassword123!"
        };
        var authResponse = new AuthResponseDto
        {
            IsSuccess = false,
            Message = "Incorrect password"
        };

        _mockAuthService
            .Setup(x => x.ChangePasswordAsync(userId, changePasswordDto))
            .ReturnsAsync(authResponse);

        // Setup the controller's User claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        _authController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };

        // Act
        var result = await _authController.ChangePassword(changePasswordDto);

        // Assert
        Assert.That(result, Is.TypeOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
    }

    #endregion

    #region UserNameExists Tests

    [Test]
    public async Task UserNameExists_WithExistingUserName_ReturnsTrue()
    {
        // Arrange
        var userName = "existinguser";

        _mockAuthService
            .Setup(x => x.UserNameExistsAsync(userName))
            .ReturnsAsync(true);

        // Act
        var result = await _authController.UserNameExists(userName);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult?.Value, Is.Not.Null);
        _mockAuthService.Verify(x => x.UserNameExistsAsync(userName), Times.Once);
    }

    [Test]
    public async Task UserNameExists_WithNonExistentUserName_ReturnsFalse()
    {
        // Arrange
        var userName = "nonexistentuser";

        _mockAuthService
            .Setup(x => x.UserNameExistsAsync(userName))
            .ReturnsAsync(false);

        // Act
        var result = await _authController.UserNameExists(userName);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }

    #endregion
}
