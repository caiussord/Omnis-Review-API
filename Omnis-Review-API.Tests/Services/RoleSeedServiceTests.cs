using Moq;
using NUnit.Framework;
using OmnisReview.Services;
using OmnisReview.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace OmnisReview.Tests.Services;

[TestFixture]
public class RoleSeedServiceTests
{
    private Mock<RoleManager<IdentityRole<Guid>>> _mockRoleManager = null!;
    private Mock<ILogger<RoleSeedService>> _mockLogger = null!;
    private RoleSeedService _roleSeedService = null!;

    [SetUp]
    public void SetUp()
    {
        _mockRoleManager = new Mock<RoleManager<IdentityRole<Guid>>>(
            new Mock<IRoleStore<IdentityRole<Guid>>>().Object,
            new IRoleValidator<IdentityRole<Guid>>[0],
            new Mock<ILookupNormalizer>().Object,
            new IdentityErrorDescriber(),
            new Mock<ILogger<RoleManager<IdentityRole<Guid>>>>().Object
        )
        {
            CallBase = false
        };

        _mockLogger = new Mock<ILogger<RoleSeedService>>();
        _roleSeedService = new RoleSeedService(_mockRoleManager.Object, _mockLogger.Object);
    }

    [Test]
    public async Task SeedRolesAsync_WithNonExistentRoles_CreatesAllRoles()
    {
        _mockRoleManager
            .Setup(r => r.RoleExistsAsync("Admin"))
            .ReturnsAsync(false);

        _mockRoleManager
            .Setup(r => r.RoleExistsAsync("User"))
            .ReturnsAsync(false);

        _mockRoleManager
            .Setup(r => r.CreateAsync(It.IsAny<IdentityRole<Guid>>()))
            .ReturnsAsync(IdentityResult.Success);

        await _roleSeedService.SeedRolesAsync();

        _mockRoleManager.Verify(
            r => r.CreateAsync(It.IsAny<IdentityRole<Guid>>()), 
            Times.Exactly(2));
    }

    [Test]
    public async Task SeedRolesAsync_WithExistingRoles_DoesNotCreateDuplicates()
    {
        _mockRoleManager
            .Setup(r => r.RoleExistsAsync("Admin"))
            .ReturnsAsync(true);

        _mockRoleManager
            .Setup(r => r.RoleExistsAsync("User"))
            .ReturnsAsync(true);

        await _roleSeedService.SeedRolesAsync();

        _mockRoleManager.Verify(
            r => r.CreateAsync(It.IsAny<IdentityRole<Guid>>()), 
            Times.Never);
    }

    [Test]
    public async Task SeedRolesAsync_WithPartialExistingRoles_CreatesOnlyMissing()
    {
        _mockRoleManager
            .Setup(r => r.RoleExistsAsync("Admin"))
            .ReturnsAsync(true);

        _mockRoleManager
            .Setup(r => r.RoleExistsAsync("User"))
            .ReturnsAsync(false);

        _mockRoleManager
            .Setup(r => r.CreateAsync(It.IsAny<IdentityRole<Guid>>()))
            .ReturnsAsync(IdentityResult.Success);

        await _roleSeedService.SeedRolesAsync();

        _mockRoleManager.Verify(
            r => r.CreateAsync(It.IsAny<IdentityRole<Guid>>()), 
            Times.Once);
    }

    [Test]
    public async Task SeedRolesAsync_WhenCreationFails_LogsError()
    {
        _mockRoleManager
            .Setup(r => r.RoleExistsAsync("Admin"))
            .ReturnsAsync(false);

        var errors = new[] { new IdentityError { Code = "DuplicateRoleName", Description = "Role already exists" } };
        _mockRoleManager
            .Setup(r => r.CreateAsync(It.IsAny<IdentityRole<Guid>>()))
            .ReturnsAsync(IdentityResult.Failed(errors));

        await _roleSeedService.SeedRolesAsync();

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);
    }
}
