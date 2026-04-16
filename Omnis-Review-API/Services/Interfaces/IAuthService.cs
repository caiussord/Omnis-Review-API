using OmnisReview.Models;
using OmnisReview.Models.Auth;

namespace OmnisReview.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginDto model);
    Task<AuthResponseDto> RegisterAsync(RegisterDto model);
    Task<bool> UserNameExistsAsync(string userName);
    Task<AuthResponseDto> ChangePasswordAsync(Guid userId, ChangePasswordDto model);
    Task<AuthResponseDto> ForgotPasswordAsync(ForgotPasswordDto model);
    Task<AuthResponseDto> ResetPasswordAsync(ResetPasswordDto model);
    Task<UserDto?> GetUserByIdAsync(Guid userId);
    Task<AuthResponseDto> AssignRoleAsync(Guid userId, string roleName);
    Task<AuthResponseDto> RemoveRoleAsync(Guid userId, string roleName);
}
