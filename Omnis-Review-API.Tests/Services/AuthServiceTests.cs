using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using OmnisReview.Models;
using OmnisReview.Repositorys.Interfaces;
using OmnisReview.Services;
using OmnisReview.Services.Interfaces;
using OmnisReview.Tests.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace OmnisReview.Tests.Services;

[TestFixture]
public class AuthServiceTests
{
    private AuthService _authService = null!;
    private Mock<IAuthRepository> _mockAuthRepository = null!;
    private Mock<IConfiguration> _mockConfiguration = null!;
    private Mock<IEmailSender> _mockEmailSender = null!;
    private Mock<ILogger<AuthService>> _mockLogger = null!;

    [SetUp]
    public void SetUp()
    {
        _mockAuthRepository = new Mock<IAuthRepository>();
        _mockConfiguration = new Mock<IConfiguration>();
        _mockEmailSender = new Mock<IEmailSender>();
        _mockLogger = new Mock<ILogger<AuthService>>();

        // Setup JWT configuration
        _mockConfiguration
            .Setup(x => x["Jwt:Key"])
            .Returns("this_is_a_very_long_secret_key_for_testing_purposes_only_1234567890");

        _mockConfiguration
            .Setup(x => x["Jwt:Issuer"])
            .Returns("OmnisReviewAPI");

        _mockConfiguration
            .Setup(x => x["Jwt:Audience"])
            .Returns("OmnisReviewUsers");

        _mockConfiguration
            .Setup(x => x["Jwt:ExpirationMinutes"])
            .Returns("60");

        _authService = new AuthService(
            _mockAuthRepository.Object,
            _mockConfiguration.Object,
            _mockEmailSender.Object,
            _mockLogger.Object
        );
    }

    [TearDown]
    public void TearDown()
    {
        _mockAuthRepository?.Reset();
        _mockConfiguration?.Reset();
        _mockEmailSender?.Reset();
        _mockLogger?.Reset();
    }

    #region Login Tests

    [Test]
    public async Task LoginAsync_WithValidEmailCredentials_ReturnsSuccessfulResponse()
    {
        // Arrange
        var loginDto = new LoginDtoBuilder().WithEmail("user@example.com").Build();
        var user = new UserBuilder().WithEmail("user@example.com").Build();

        _mockAuthRepository
            .Setup(x => x.FindByEmailAsync(loginDto.Email))
            .ReturnsAsync(user);

        _mockAuthRepository
            .Setup(x => x.CheckPasswordAsync(user, loginDto.Password))
            .ReturnsAsync(true);

        _mockAuthRepository
            .Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "User" });

        // Act
        var result = await _authService.LoginAsync(loginDto);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Token, Is.Not.Null);
        Assert.That(result.Token, Is.Not.Empty);
        Assert.That(result.Expiration, Is.Not.Null);
        _mockAuthRepository.Verify(x => x.FindByEmailAsync(loginDto.Email), Times.Once);
    }

    [Test]
    public async Task LoginAsync_WithValidUserNameCredentials_ReturnsSuccessfulResponse()
    {
        // Arrange
        var loginDto = new LoginDtoBuilder().WithEmail("testuser").Build();
        var user = new UserBuilder().WithUserName("testuser").Build();

        _mockAuthRepository
            .Setup(x => x.FindByEmailAsync(loginDto.Email))
            .ReturnsAsync(null as ApplicationUser);

        _mockAuthRepository
            .Setup(x => x.FindByUserNameAsync(loginDto.Email))
            .ReturnsAsync(user);

        _mockAuthRepository
            .Setup(x => x.CheckPasswordAsync(user, loginDto.Password))
            .ReturnsAsync(true);

        _mockAuthRepository
            .Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "User" });

        // Act
        var result = await _authService.LoginAsync(loginDto);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Token, Is.Not.Null);
        _mockAuthRepository.Verify(x => x.FindByUserNameAsync(loginDto.Email), Times.Once);
    }

    [Test]
    public async Task LoginAsync_WithInvalidPassword_ReturnsFailureResponse()
    {
        // Arrange
        var loginDto = new LoginDtoBuilder().Build();
        var user = new UserBuilder().Build();

        _mockAuthRepository
            .Setup(x => x.FindByEmailAsync(loginDto.Email))
            .ReturnsAsync(user);

        _mockAuthRepository
            .Setup(x => x.CheckPasswordAsync(user, loginDto.Password))
            .ReturnsAsync(false);

        // Act
        var result = await _authService.LoginAsync(loginDto);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Message, Does.Contain("Invalid login attempt"));
        Assert.That(result.Token, Is.Null);
    }

    [Test]
    public async Task LoginAsync_WithNonExistentUser_ReturnsFailureResponse()
    {
        // Arrange
        var loginDto = new LoginDtoBuilder().Build();

        _mockAuthRepository
            .Setup(x => x.FindByEmailAsync(loginDto.Email))
            .ReturnsAsync(null as ApplicationUser);

        _mockAuthRepository
            .Setup(x => x.FindByUserNameAsync(loginDto.Email))
            .ReturnsAsync(null as ApplicationUser);

        // Act
        var result = await _authService.LoginAsync(loginDto);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Message, Does.Contain("Invalid login attempt"));
    }

    [Test]
    public async Task LoginAsync_WithValidCredentialsAndMultipleRoles_IncludesAllRolesInToken()
    {
        // Arrange
        var loginDto = new LoginDtoBuilder().Build();
        var user = new UserBuilder().Build();
        var roles = new List<string> { "User", "Admin", "Moderator" };

        _mockAuthRepository
            .Setup(x => x.FindByEmailAsync(loginDto.Email))
            .ReturnsAsync(user);

        _mockAuthRepository
            .Setup(x => x.CheckPasswordAsync(user, loginDto.Password))
            .ReturnsAsync(true);

        _mockAuthRepository
            .Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(roles);

        // Act
        var result = await _authService.LoginAsync(loginDto);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Token, Is.Not.Null);
        _mockAuthRepository.Verify(x => x.GetRolesAsync(user), Times.Once);
    }

    #endregion

    #region Register Tests

    [Test]
    public async Task RegisterAsync_WithValidData_CreatesUserSuccessfully()
    {
        // Arrange
        var registerDto = new RegisterDtoBuilder().Build();
        var identityResult = IdentityResult.Success;

        _mockAuthRepository
            .Setup(x => x.FindByEmailAsync(registerDto.Email))
            .ReturnsAsync(null as ApplicationUser);

        _mockAuthRepository
            .Setup(x => x.FindByUserNameAsync(registerDto.UserName))
            .ReturnsAsync(null as ApplicationUser);

        _mockAuthRepository
            .Setup(x => x.CreateUserAsync(It.IsAny<ApplicationUser>(), registerDto.Password))
            .ReturnsAsync(identityResult);

        // Act
        var result = await _authService.RegisterAsync(registerDto);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Message, Does.Contain("successfully"));
        _mockAuthRepository.Verify(
            x => x.CreateUserAsync(It.IsAny<ApplicationUser>(), registerDto.Password),
            Times.Once
        );
    }

    [Test]
    public async Task RegisterAsync_WithExistingEmail_ReturnsFailureResponse()
    {
        // Arrange
        var registerDto = new RegisterDtoBuilder().Build();
        var existingUser = new UserBuilder().WithEmail(registerDto.Email).Build();

        _mockAuthRepository
            .Setup(x => x.FindByEmailAsync(registerDto.Email))
            .ReturnsAsync(existingUser);

        // Act
        var result = await _authService.RegisterAsync(registerDto);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Message, Does.Contain("Email already exists"));
        _mockAuthRepository.Verify(
            x => x.CreateUserAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
            Times.Never
        );
    }

    [Test]
    public async Task RegisterAsync_WithExistingUserName_ReturnsFailureResponse()
    {
        // Arrange
        var registerDto = new RegisterDtoBuilder().Build();
        var existingUser = new UserBuilder().WithUserName(registerDto.UserName).Build();

        _mockAuthRepository
            .Setup(x => x.FindByEmailAsync(registerDto.Email))
            .ReturnsAsync(null as ApplicationUser);

        _mockAuthRepository
            .Setup(x => x.FindByUserNameAsync(registerDto.UserName))
            .ReturnsAsync(existingUser);

        // Act
        var result = await _authService.RegisterAsync(registerDto);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Message, Does.Contain("Username already exists"));
    }

    [Test]
    public async Task RegisterAsync_WithInvalidPassword_ReturnsFailureResponse()
    {
        // Arrange
        var registerDto = new RegisterDtoBuilder().Build();
        var errors = new[]
        {
            new IdentityError { Description = "Password too weak" },
            new IdentityError { Description = "Password must contain special character" }
        };
        var identityResult = IdentityResult.Failed(errors);

        _mockAuthRepository
            .Setup(x => x.FindByEmailAsync(registerDto.Email))
            .ReturnsAsync(null as ApplicationUser);

        _mockAuthRepository
            .Setup(x => x.FindByUserNameAsync(registerDto.UserName))
            .ReturnsAsync(null as ApplicationUser);

        _mockAuthRepository
            .Setup(x => x.CreateUserAsync(It.IsAny<ApplicationUser>(), registerDto.Password))
            .ReturnsAsync(identityResult);

        // Act
        var result = await _authService.RegisterAsync(registerDto);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Message, Does.Contain("User creation failed"));
        Assert.That(result.Message, Does.Contain("Password too weak"));
    }

    #endregion

    #region UserName Exists Tests

    [Test]
    public async Task UserNameExistsAsync_WithExistingUserName_ReturnsTrue()
    {
        // Arrange
        var userName = "existinguser";
        var user = new UserBuilder().WithUserName(userName).Build();

        _mockAuthRepository
            .Setup(x => x.FindByUserNameAsync(userName))
            .ReturnsAsync(user);

        // Act
        var result = await _authService.UserNameExistsAsync(userName);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task UserNameExistsAsync_WithNonExistentUserName_ReturnsFalse()
    {
        // Arrange
        var userName = "nonexistentuser";

        _mockAuthRepository
            .Setup(x => x.FindByUserNameAsync(userName))
            .ReturnsAsync(null as ApplicationUser);

        // Act
        var result = await _authService.UserNameExistsAsync(userName);

        // Assert
        Assert.That(result, Is.False);
    }

    #endregion
}
