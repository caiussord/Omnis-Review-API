using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OmnisReview.Models;
using OmnisReview.Repositorys.Interfaces;
using OmnisReview.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.WebUtilities;

namespace OmnisReview.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IConfiguration _configuration;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IAuthRepository authRepository, IConfiguration configuration, IEmailSender emailSender, ILogger<AuthService> logger)
    {
        _authRepository = authRepository;
        _configuration = configuration;
        _emailSender = emailSender;
        _logger = logger;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto model)
    {
        var user = await _authRepository.FindByEmailAsync(model.Email);
        if (user == null)
        {
            user = await _authRepository.FindByUserNameAsync(model.Email);
        }
        
        if (user != null && await _authRepository.CheckPasswordAsync(user, model.Password))
        {
            var userRoles = await _authRepository.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GetToken(authClaims);

            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "User logged in successfully.",
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            };
        }

        return new AuthResponseDto { IsSuccess = false, Message = "Invalid login attempt" };
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto model)
    {
        var userExists = await _authRepository.FindByEmailAsync(model.Email);
        if (userExists != null)
            return new AuthResponseDto { IsSuccess = false, Message = "Email already exists" };

        var userNameExists = await _authRepository.FindByUserNameAsync(model.UserName);
        if (userNameExists != null)
            return new AuthResponseDto { IsSuccess = false, Message = "Username already exists" };

        ApplicationUser user = new()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.UserName,
            Name = model.Name,
            Birth_Date = model.Birth_Date
        };

        var result = await _authRepository.CreateUserAsync(user, model.Password);
        
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AuthResponseDto { IsSuccess = false, Message = $"User creation failed Errors: {errors}" };
        }

        return new AuthResponseDto { IsSuccess = true, Message = "User created successfully" };
    }

    public async Task<bool> UserNameExistsAsync(string userName)
    {
        var user = await _authRepository.FindByUserNameAsync(userName);
        return user != null;
    }

    public async Task<AuthResponseDto> ChangePasswordAsync(Guid userId, ChangePasswordDto model)
    {
        var user = await _authRepository.FindByIdAsync(userId);
        if (user == null)
            return new AuthResponseDto { IsSuccess = false, Message = "User not found" };

        var result = await _authRepository.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AuthResponseDto { IsSuccess = false, Message = $"Change password failed Errors: {errors}" };
        }

        return new AuthResponseDto { IsSuccess = true, Message = "Password changed successfully" };
    }

    public async Task<AuthResponseDto> ForgotPasswordAsync(ForgotPasswordDto model)
    {
        var user = await _authRepository.FindByEmailAsync(model.UserNameOrEmail);
        if (user == null)
        {
            user = await _authRepository.FindByUserNameAsync(model.UserNameOrEmail);
        }

        if (user == null)
        {
            _logger.LogInformation("Forgot password requested for unknown user: {UserNameOrEmail}", model.UserNameOrEmail);
            return new AuthResponseDto { IsSuccess = true, Message = "If the account exists, a reset link was sent." };
        }

        if (string.IsNullOrWhiteSpace(user.Email))
        {
            _logger.LogWarning("Forgot password requested for user without email: {UserId}", user.Id);
            return new AuthResponseDto { IsSuccess = false, Message = "User does not have an email." };
        }

        var resetPasswordUrl = _configuration["Frontend:ResetPasswordUrl"];
        if (string.IsNullOrWhiteSpace(resetPasswordUrl))
        {
            _logger.LogWarning("Reset password URL not configured.");
            return new AuthResponseDto { IsSuccess = false, Message = "Reset password URL not configured." };
        }

        var token = await _authRepository.GeneratePasswordResetTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var resetLink = $"{resetPasswordUrl}?userId={user.Id}&token={encodedToken}";
        var body = $"Clique no link para alterar sua senha: {resetLink}";

        _logger.LogInformation("Sending reset password email to {Email}", user.Email);
        await _emailSender.SendEmailAsync(user.Email, "Reset password", body);
        _logger.LogInformation("Reset password email sent to {Email}", user.Email);

        return new AuthResponseDto { IsSuccess = true, Message = "If the account exists, a reset link was sent." };
    }

    public async Task<AuthResponseDto> ResetPasswordAsync(ResetPasswordDto model)
    {
        var user = await _authRepository.FindByIdAsync(model.UserId);
        if (user == null)
            return new AuthResponseDto { IsSuccess = false, Message = "User not found" };

        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));
        var result = await _authRepository.ResetPasswordAsync(user, decodedToken, model.NewPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AuthResponseDto { IsSuccess = false, Message = $"Reset password failed Errors: {errors}" };
        }

        return new AuthResponseDto { IsSuccess = true, Message = "Password reset successfully" };
    }

    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

        return new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
    }
}
