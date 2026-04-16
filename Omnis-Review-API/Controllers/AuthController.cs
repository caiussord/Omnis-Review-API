using OmnisReview.Models;
using OmnisReview.Models.Auth;
using OmnisReview.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

    [HttpGet]
    [Route("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized();

        var user = await _authService.GetUserByIdAsync(userId);
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpPost]
    [Route("assign-role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto model)
    {
        if (string.IsNullOrWhiteSpace(model.UserId) || string.IsNullOrWhiteSpace(model.RoleName))
            return BadRequest(new { Message = "UserId and RoleName are required." });

        if (!Guid.TryParse(model.UserId, out var userId))
            return BadRequest(new { Message = "Invalid UserId format." });

        var result = await _authService.AssignRoleAsync(userId, model.RoleName);

        if (!result.IsSuccess)
            return StatusCode(StatusCodes.Status400BadRequest, new { Status = "Error", Message = result.Message });

        return Ok(new { Status = "Success", Message = result.Message });
    }

    [HttpPost]
    [Route("remove-role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveRole([FromBody] AssignRoleDto model)
    {
        if (string.IsNullOrWhiteSpace(model.UserId) || string.IsNullOrWhiteSpace(model.RoleName))
            return BadRequest(new { Message = "UserId and RoleName are required." });

        if (!Guid.TryParse(model.UserId, out var userId))
            return BadRequest(new { Message = "Invalid UserId format." });

        var result = await _authService.RemoveRoleAsync(userId, model.RoleName);

        if (!result.IsSuccess)
            return StatusCode(StatusCodes.Status400BadRequest, new { Status = "Error", Message = result.Message });

        return Ok(new { Status = "Success", Message = result.Message });
    }
}
