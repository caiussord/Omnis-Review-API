using OmnisReview.Models;
using OmnisReview.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OmnisReview.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var result = await _authService.LoginAsync(model);

        if (result.IsSuccess)
        {
            return Ok(new
            {
                token = result.Token,
                expiration = result.Expiration
            });
        }

        return Unauthorized(new { Message = result.Message });
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        var result = await _authService.RegisterAsync(model);

        if (!result.IsSuccess)
        {
            if (result.Message == "User already exists!")
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = result.Message });

            return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = result.Message });
        }

        return Ok(new { Status = "Success", Message = result.Message });
    }

    [HttpPost]
    [Route("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
    {
        var result = await _authService.ForgotPasswordAsync(model);

        if (!result.IsSuccess)
            return StatusCode(StatusCodes.Status400BadRequest, new { Status = "Error", Message = result.Message });

        return Ok(new { Status = "Success", Message = result.Message });
    }

    [HttpPost]
    [Route("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
    {
        var result = await _authService.ResetPasswordAsync(model);

        if (!result.IsSuccess)
            return StatusCode(StatusCodes.Status400BadRequest, new { Status = "Error", Message = result.Message });

        return Ok(new { Status = "Success", Message = result.Message });
    }

    [HttpPost]
    [Route("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
    {
        var userIdValue = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdValue, out var userId))
            return Unauthorized(new { Status = "Error", Message = "User not authenticated." });

        var result = await _authService.ChangePasswordAsync(userId, model);

        if (!result.IsSuccess)
            return StatusCode(StatusCodes.Status400BadRequest, new { Status = "Error", Message = result.Message });

        return Ok(new { Status = "Success", Message = result.Message });
    }

    [HttpGet]
    [Route("username-exists")]
    public async Task<IActionResult> UserNameExists([FromQuery] string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            return BadRequest(new { Message = "Username is required." });

        var exists = await _authService.UserNameExistsAsync(userName);
        return Ok(new { exists });
    }
}
