using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OmnisReview.Models;
using OmnisReview.Services.Interfaces;

namespace OmnisReview.Services;

public class UserSeedService : IUserSeedService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UserSeedService> _logger;

    private static readonly Guid AdminUserId = Guid.Parse("66A3BD00-3F9F-49DF-A047-FFC97E4677E6");
    private const string AdminRoleName = "Admin";

    public UserSeedService(UserManager<ApplicationUser> userManager, ILogger<UserSeedService> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task SeedAdminUserAsync()
    {
        try
        {
            var adminUser = await _userManager.FindByIdAsync(AdminUserId.ToString());

            if (adminUser == null)
            {
                _logger.LogWarning("Admin user with ID {AdminUserId} not found in database", AdminUserId);
                return;
            }

            var isAdmin = await _userManager.IsInRoleAsync(adminUser, AdminRoleName);

            if (isAdmin)
            {
                _logger.LogInformation("User {UserName} is already an Admin", adminUser.UserName);
                return;
            }

            var result = await _userManager.AddToRoleAsync(adminUser, AdminRoleName);

            if (result.Succeeded)
            {
                _logger.LogInformation("User {UserName} has been promoted to Admin role", adminUser.UserName);
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to promote user {UserName} to Admin: {Errors}", adminUser.UserName, errors);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error seeding admin user");
        }
    }
}
