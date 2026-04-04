using OmnisReview.Models;
using Microsoft.AspNetCore.Identity;

namespace OmnisReview.Repositorys.Interfaces;

public interface IAuthRepository
{
    Task<ApplicationUser?> FindByEmailAsync(string email);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    Task<IList<string>> GetRolesAsync(ApplicationUser user);
    Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
}
