using OmnisReview.Models;
using OmnisReview.Services.Interfaces;
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
}
