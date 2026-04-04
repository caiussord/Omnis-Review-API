using OmnisReview.Models;

namespace OmnisReview.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginDto model);
    Task<AuthResponseDto> RegisterAsync(RegisterDto model);
    Task<bool> UserNameExistsAsync(string userName);
    Task<AuthResponseDto> ChangePasswordAsync(Guid userId, ChangePasswordDto model);
    Task<AuthResponseDto> ForgotPasswordAsync(ForgotPasswordDto model);
    Task<AuthResponseDto> ResetPasswordAsync(ResetPasswordDto model);
}
