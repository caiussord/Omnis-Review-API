using Microsoft.AspNetCore.Mvc;
using OmnisReview.Services.Interfaces;

namespace OmnisReview.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TmdbController : ControllerBase
{
    private readonly ITmdbService _tmdbService;

    public TmdbController(ITmdbService tmdbService)
    {
        _tmdbService = tmdbService;
    }

    [HttpGet("movies/search")]
    public async Task<IActionResult> SearchMovies([FromQuery] string query, [FromQuery] int page = 1)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query parameter is required");

        var result = await _tmdbService.SearchMoviesAsync(query, page);
        return Ok(result);
    }

    [HttpGet("series/search")]
    public async Task<IActionResult> SearchSeries([FromQuery] string query, [FromQuery] int page = 1)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query parameter is required");

        var result = await _tmdbService.SearchSeriesAsync(query, page);
        return Ok(result);
    }

    [HttpGet("movies/{id}")]
    public async Task<IActionResult> GetMovieDetails([FromRoute] int id)
    {
        var result = await _tmdbService.GetMovieDetailsAsync(id);
        return Ok(result);
    }

    [HttpGet("series/{id}")]
    public async Task<IActionResult> GetSeriesDetails([FromRoute] int id)
    {
        var result = await _tmdbService.GetSeriesDetailsAsync(id);
        return Ok(result);
    }

    [HttpGet("movies/{id}/cast")]
    public async Task<IActionResult> GetMovieCast([FromRoute] int id)
    {
        var result = await _tmdbService.GetMovieCastAsync(id);
        return Ok(result);
    }

    [HttpGet("series/{id}/cast")]
    public async Task<IActionResult> GetSeriesCast([FromRoute] int id)
    {
        var result = await _tmdbService.GetSeriesCastAsync(id);
        return Ok(result);
    }

    [HttpGet("movies/{id}/videos")]
    public async Task<IActionResult> GetMovieVideos([FromRoute] int id)
    {
        var result = await _tmdbService.GetMovieVideosAsync(id);
        return Ok(result);
    }

    [HttpGet("series/{id}/videos")]
    public async Task<IActionResult> GetSeriesVideos([FromRoute] int id)
    {
        var result = await _tmdbService.GetSeriesVideosAsync(id);
        return Ok(result);
    }

    [HttpGet("movies/popular")]
    public async Task<IActionResult> GetPopularMovies([FromQuery] int page = 1)
    {
        var result = await _tmdbService.GetPopularMoviesAsync(page);
        return Ok(result);
    }

    [HttpGet("series/popular")]
    public async Task<IActionResult> GetPopularSeries([FromQuery] int page = 1)
    {
        var result = await _tmdbService.GetPopularSeriesAsync(page);
        return Ok(result);
    }

    [HttpGet("movies/top-rated")]
    public async Task<IActionResult> GetTopRatedMovies([FromQuery] int page = 1)
    {
        var result = await _tmdbService.GetTopRatedMoviesAsync(page);
        return Ok(result);
    }

    [HttpGet("series/top-rated")]
    public async Task<IActionResult> GetTopRatedSeries([FromQuery] int page = 1)
    {
        var result = await _tmdbService.GetTopRatedSeriesAsync(page);
        return Ok(result);
    }
}
