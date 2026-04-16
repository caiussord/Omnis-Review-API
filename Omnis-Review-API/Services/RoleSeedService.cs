using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OmnisReview.Services.Interfaces;

namespace OmnisReview.Services;

public class RoleSeedService : IRoleSeedService
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly ILogger<RoleSeedService> _logger;

    private static readonly string[] DefaultRoles = { "Admin", "User" };

    public RoleSeedService(RoleManager<IdentityRole<Guid>> roleManager, ILogger<RoleSeedService> logger)
    {
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task SeedRolesAsync()
    {
        foreach (var roleName in DefaultRoles)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);

            if (roleExists)
            {
                _logger.LogInformation("Role '{RoleName}' already exists", roleName);
                continue;
            }

            var role = new IdentityRole<Guid> { Name = roleName };
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                _logger.LogInformation("Role '{RoleName}' created successfully", roleName);
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to create role '{RoleName}': {Errors}", roleName, errors);
            }
        }
    }
}
