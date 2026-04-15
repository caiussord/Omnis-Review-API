using Microsoft.AspNetCore.Mvc;
using OmnisReview.Services.Interfaces;

namespace OmnisReview.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RawgController : ControllerBase
{
    private readonly IRawgService _rawgService;

    public RawgController(IRawgService rawgService)
    {
        _rawgService = rawgService;
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchGames([FromQuery] string query, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query parameter is required");

        if (pageSize > 40)
            pageSize = 40;

        var result = await _rawgService.SearchGamesAsync(query, page, pageSize);
        return Ok(result);
    }

    [HttpGet("search/genre")]
    public async Task<IActionResult> SearchByGenre([FromQuery] string genre, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (string.IsNullOrWhiteSpace(genre))
            return BadRequest("Genre parameter is required");

        if (pageSize > 40)
            pageSize = 40;

        var result = await _rawgService.SearchGamesByGenreAsync(genre, page, pageSize);
        return Ok(result);
    }

    [HttpGet("search/platform")]
    public async Task<IActionResult> SearchByPlatform([FromQuery] string platform, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (string.IsNullOrWhiteSpace(platform))
            return BadRequest("Platform parameter is required");

        if (pageSize > 40)
            pageSize = 40;

        var result = await _rawgService.SearchGamesByPlatformAsync(platform, page, pageSize);
        return Ok(result);
    }

    [HttpGet("popular")]
    public async Task<IActionResult> GetPopularGames([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (pageSize > 40)
            pageSize = 40;

        var result = await _rawgService.GetPopularGamesAsync(page, pageSize);
        return Ok(result);
    }

    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcomingGames([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (pageSize > 40)
            pageSize = 40;

        var result = await _rawgService.GetUpcomingGamesAsync(page, pageSize);
        return Ok(result);
    }

    [HttpGet("{gameId}")]
    public async Task<IActionResult> GetGameDetails([FromRoute] int gameId)
    {
        if (gameId <= 0)
            return BadRequest("Game ID must be a positive number");

        var result = await _rawgService.GetGameByIdAsync(gameId);
        
        if (result is null)
            return NotFound("Game not found");

        return Ok(result);
    }

    [HttpGet("sort/{sortBy}")]
    public async Task<IActionResult> GetGamesBySorted([FromRoute] string sortBy, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
            return BadRequest("Sort parameter is required");

        if (pageSize > 40)
            pageSize = 40;

        var result = await _rawgService.GetGamesBySortAsync(sortBy, page, pageSize);
        return Ok(result);
    }

    [HttpGet("developers")]
    public async Task<IActionResult> GetDevelopers([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (pageSize > 40)
            pageSize = 40;

        var result = await _rawgService.GetDevelopersAsync(page, pageSize);
        return Ok(result);
    }

    [HttpGet("developers/{developerId}")]
    public async Task<IActionResult> GetDeveloperDetails([FromRoute] int developerId)
    {
        if (developerId <= 0)
            return BadRequest("Developer ID must be a positive number");

        var result = await _rawgService.GetDeveloperByIdAsync(developerId);

        if (result is null)
            return NotFound("Developer not found");

        return Ok(result);
    }

    [HttpGet("publishers")]
    public async Task<IActionResult> GetPublishers([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (pageSize > 40)
            pageSize = 40;

        var result = await _rawgService.GetPublishersAsync(page, pageSize);
        return Ok(result);
    }

    [HttpGet("publishers/{publisherId}")]
    public async Task<IActionResult> GetPublisherDetails([FromRoute] int publisherId)
    {
        if (publisherId <= 0)
            return BadRequest("Publisher ID must be a positive number");

        var result = await _rawgService.GetPublisherByIdAsync(publisherId);

        if (result is null)
            return NotFound("Publisher not found");

        return Ok(result);
    }

    [HttpGet("games/search/card")]
    public async Task<IActionResult> SearchGamesCard([FromQuery] string query, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query parameter is required");

        if (pageSize > 40)
            pageSize = 40;

        var result = await _rawgService.SearchGamesCardAsync(query, page, pageSize);
        if (result == null)
            return NotFound("Nenhum jogo encontrado");

        return Ok(result);
    }

    [HttpGet("games/{gameId}/detail")]
    public async Task<IActionResult> GetGameDetail([FromRoute] int gameId)
    {
        if (gameId <= 0)
            return BadRequest("Game ID must be a positive number");

        var result = await _rawgService.GetGameDetailAsync(gameId);
        if (result == null)
            return NotFound("Jogo não encontrado");

        return Ok(result);
    }

    [HttpGet("games/popular/card")]
    public async Task<IActionResult> GetPopularGamesCard([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (pageSize > 40)
            pageSize = 40;

        var result = await _rawgService.GetPopularGamesCardAsync(page, pageSize);
        if (result == null)
            return NotFound("Nenhum jogo popular encontrado");

        return Ok(result);
    }
}
