using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CCSS_API.Models;
using CCSS_API.Repositorys.Interfaces;
using CCSS_API.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace CCSS_API.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IAuthRepository authRepository, IConfiguration configuration)
    {
        _authRepository = authRepository;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto model)
    {
        var user = await _authRepository.FindByEmailAsync(model.Email);
        
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
            return new AuthResponseDto { IsSuccess = false, Message = "User already exists!" };

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
