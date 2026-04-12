using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using OmnisReview.Models;
using OmnisReview.Repositorys;
using OmnisReview.Tests.Helpers;
using Microsoft.AspNetCore.Identity;

namespace OmnisReview.Tests.Repositorys;

[TestFixture]
public class AuthRepositoryTests
{
    private AuthRepository _authRepository = null!;
    private Mock<UserManager<ApplicationUser>> _mockUserManager = null!;

    [SetUp]
    public void SetUp()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(
            store.Object,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!
        );

        _authRepository = new AuthRepository(_mockUserManager.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _mockUserManager?.Reset();
    }

    #region FindByEmailAsync Tests

    [Test]
    public async Task FindByEmailAsync_WithExistingEmail_ReturnsUser()
    {
        // Arrange
        var email = "user@example.com";
        var user = new UserBuilder().WithEmail(email).Build();

        _mockUserManager
            .Setup(x => x.FindByEmailAsync(email))
            .ReturnsAsync(user);

        // Act
        var result = await _authRepository.FindByEmailAsync(email);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Email, Is.EqualTo(email));
        _mockUserManager.Verify(x => x.FindByEmailAsync(email), Times.Once);
    }

    [Test]
    public async Task FindByEmailAsync_WithNonExistentEmail_ReturnsNull()
    {
        // Arrange
        var email = "nonexistent@example.com";

        _mockUserManager
            .Setup(x => x.FindByEmailAsync(email))
            .ReturnsAsync(null as ApplicationUser);

        // Act
        var result = await _authRepository.FindByEmailAsync(email);

        // Assert
        Assert.That(result, Is.Null);
    }

    #endregion

    #region FindByUserNameAsync Tests

    [Test]
    public async Task FindByUserNameAsync_WithExistingUserName_ReturnsUser()
    {
        // Arrange
        var userName = "testuser";
        var user = new UserBuilder().WithUserName(userName).Build();

        _mockUserManager
            .Setup(x => x.FindByNameAsync(userName))
            .ReturnsAsync(user);

        // Act
        var result = await _authRepository.FindByUserNameAsync(userName);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.UserName, Is.EqualTo(userName));
    }

    [Test]
    public async Task FindByUserNameAsync_WithNonExistentUserName_ReturnsNull()
    {
        // Arrange
        var userName = "nonexistentuser";

        _mockUserManager
            .Setup(x => x.FindByNameAsync(userName))
            .ReturnsAsync(null as ApplicationUser);

        // Act
        var result = await _authRepository.FindByUserNameAsync(userName);

        // Assert
        Assert.That(result, Is.Null);
    }

    #endregion

    #region FindByIdAsync Tests

    [Test]
    public async Task FindByIdAsync_WithExistingId_ReturnsUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new UserBuilder().Build();
        user.Id = userId;

        _mockUserManager
            .Setup(x => x.FindByIdAsync(userId.ToString()))
            .ReturnsAsync(user);

        // Act
        var result = await _authRepository.FindByIdAsync(userId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Id, Is.EqualTo(userId));
    }

    [Test]
    public async Task FindByIdAsync_WithNonExistentId_ReturnsNull()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _mockUserManager
            .Setup(x => x.FindByIdAsync(userId.ToString()))
            .ReturnsAsync(null as ApplicationUser);

        // Act
        var result = await _authRepository.FindByIdAsync(userId);

        // Assert
        Assert.That(result, Is.Null);
    }

    #endregion

    #region CheckPasswordAsync Tests

    [Test]
    public async Task CheckPasswordAsync_WithCorrectPassword_ReturnsTrue()
    {
        // Arrange
        var user = new UserBuilder().Build();
        var password = "Password123!";

        _mockUserManager
            .Setup(x => x.CheckPasswordAsync(user, password))
            .ReturnsAsync(true);

        // Act
        var result = await _authRepository.CheckPasswordAsync(user, password);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task CheckPasswordAsync_WithIncorrectPassword_ReturnsFalse()
    {
        // Arrange
        var user = new UserBuilder().Build();
        var password = "WrongPassword123!";

        _mockUserManager
            .Setup(x => x.CheckPasswordAsync(user, password))
            .ReturnsAsync(false);

        // Act
        var result = await _authRepository.CheckPasswordAsync(user, password);

        // Assert
        Assert.That(result, Is.False);
    }

    #endregion

    #region GetRolesAsync Tests

    [Test]
    public async Task GetRolesAsync_WithUserHavingRoles_ReturnsRoles()
    {
        // Arrange
        var user = new UserBuilder().Build();
        var roles = new List<string> { "User", "Admin" };

        _mockUserManager
            .Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(roles);

        // Act
        var result = await _authRepository.GetRolesAsync(user);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result, Contains.Item("User"));
        Assert.That(result, Contains.Item("Admin"));
    }

    [Test]
    public async Task GetRolesAsync_WithUserHavingNoRoles_ReturnsEmptyList()
    {
        // Arrange
        var user = new UserBuilder().Build();
        var roles = new List<string>();

        _mockUserManager
            .Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(roles);

        // Act
        var result = await _authRepository.GetRolesAsync(user);

        // Assert
        Assert.That(result, Is.Empty);
    }

    #endregion

    #region CreateUserAsync Tests

    [Test]
    public async Task CreateUserAsync_WithValidData_ReturnsSuccessResult()
    {
        // Arrange
        var user = new UserBuilder().Build();
        var password = "Password123!";
        var identityResult = IdentityResult.Success;

        _mockUserManager
            .Setup(x => x.CreateAsync(user, password))
            .ReturnsAsync(identityResult);

        // Act
        var result = await _authRepository.CreateUserAsync(user, password);

        // Assert
        Assert.That(result.Succeeded, Is.True);
        _mockUserManager.Verify(x => x.CreateAsync(user, password), Times.Once);
    }

    [Test]
    public async Task CreateUserAsync_WithInvalidPassword_ReturnsFailureResult()
    {
        // Arrange
        var user = new UserBuilder().Build();
        var password = "weak";
        var errors = new[]
        {
            new IdentityError { Description = "Password too short" }
        };
        var identityResult = IdentityResult.Failed(errors);

        _mockUserManager
            .Setup(x => x.CreateAsync(user, password))
            .ReturnsAsync(identityResult);

        // Act
        var result = await _authRepository.CreateUserAsync(user, password);

        // Assert
        Assert.That(result.Succeeded, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
    }

    #endregion

    #region ChangePasswordAsync Tests

    [Test]
    public async Task ChangePasswordAsync_WithCorrectOldPassword_ReturnsSuccessResult()
    {
        // Arrange
        var user = new UserBuilder().Build();
        var currentPassword = "OldPassword123!";
        var newPassword = "NewPassword123!";
        var identityResult = IdentityResult.Success;

        _mockUserManager
            .Setup(x => x.ChangePasswordAsync(user, currentPassword, newPassword))
            .ReturnsAsync(identityResult);

        // Act
        var result = await _authRepository.ChangePasswordAsync(user, currentPassword, newPassword);

        // Assert
        Assert.That(result.Succeeded, Is.True);
        _mockUserManager.Verify(
            x => x.ChangePasswordAsync(user, currentPassword, newPassword),
            Times.Once
        );
    }

    [Test]
    public async Task ChangePasswordAsync_WithIncorrectOldPassword_ReturnsFailureResult()
    {
        // Arrange
        var user = new UserBuilder().Build();
        var currentPassword = "WrongPassword123!";
        var newPassword = "NewPassword123!";
        var errors = new[]
        {
            new IdentityError { Description = "Incorrect password" }
        };
        var identityResult = IdentityResult.Failed(errors);

        _mockUserManager
            .Setup(x => x.ChangePasswordAsync(user, currentPassword, newPassword))
            .ReturnsAsync(identityResult);

        // Act
        var result = await _authRepository.ChangePasswordAsync(user, currentPassword, newPassword);

        // Assert
        Assert.That(result.Succeeded, Is.False);
    }

    #endregion

    #region GeneratePasswordResetTokenAsync Tests

    [Test]
    public async Task GeneratePasswordResetTokenAsync_WithValidUser_ReturnsToken()
    {
        // Arrange
        var user = new UserBuilder().Build();
        var token = "reset-token-123";

        _mockUserManager
            .Setup(x => x.GeneratePasswordResetTokenAsync(user))
            .ReturnsAsync(token);

        // Act
        var result = await _authRepository.GeneratePasswordResetTokenAsync(user);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Is.EqualTo(token));
    }

    #endregion

    #region ResetPasswordAsync Tests

    [Test]
    public async Task ResetPasswordAsync_WithValidToken_ReturnsSuccessResult()
    {
        // Arrange
        var user = new UserBuilder().Build();
        var token = "reset-token-123";
        var newPassword = "NewPassword123!";
        var identityResult = IdentityResult.Success;

        _mockUserManager
            .Setup(x => x.ResetPasswordAsync(user, token, newPassword))
            .ReturnsAsync(identityResult);

        // Act
        var result = await _authRepository.ResetPasswordAsync(user, token, newPassword);

        // Assert
        Assert.That(result.Succeeded, Is.True);
        _mockUserManager.Verify(x => x.ResetPasswordAsync(user, token, newPassword), Times.Once);
    }

    [Test]
    public async Task ResetPasswordAsync_WithInvalidToken_ReturnsFailureResult()
    {
        // Arrange
        var user = new UserBuilder().Build();
        var token = "invalid-token";
        var newPassword = "NewPassword123!";
        var errors = new[]
        {
            new IdentityError { Description = "Invalid token" }
        };
        var identityResult = IdentityResult.Failed(errors);

        _mockUserManager
            .Setup(x => x.ResetPasswordAsync(user, token, newPassword))
            .ReturnsAsync(identityResult);

        // Act
        var result = await _authRepository.ResetPasswordAsync(user, token, newPassword);

        // Assert
        Assert.That(result.Succeeded, Is.False);
    }

    #endregion
}
