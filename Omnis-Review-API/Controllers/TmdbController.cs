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

    [HttpGet("series/{seriesId}/season/{seasonNumber}")]
    public async Task<IActionResult> GetSeason([FromRoute] int seriesId, [FromRoute] int seasonNumber)
    {
        var result = await _tmdbService.GetSeasonAsync(seriesId, seasonNumber);
        return Ok(result);
    }

    [HttpGet("series/{seriesId}/season/{seasonNumber}/episode/{episodeNumber}")]
    public async Task<IActionResult> GetEpisode([FromRoute] int seriesId, [FromRoute] int seasonNumber, [FromRoute] int episodeNumber)
    {
        var result = await _tmdbService.GetEpisodeAsync(seriesId, seasonNumber, episodeNumber);
        return Ok(result);
    }

    [HttpGet("movies/search/card")]
    public async Task<IActionResult> SearchMoviesCard([FromQuery] string query, [FromQuery] int page = 1)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query parameter is required");

        var result = await _tmdbService.SearchMoviesCardAsync(query, page);
        if (result == null)
            return NotFound("Nenhum filme encontrado");

        return Ok(result);
    }

    [HttpGet("series/search/card")]
    public async Task<IActionResult> SearchSeriesCard([FromQuery] string query, [FromQuery] int page = 1)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query parameter is required");

        var result = await _tmdbService.SearchSeriesCardAsync(query, page);
        if (result == null)
            return NotFound("Nenhuma série encontrada");

        return Ok(result);
    }

    [HttpGet("movies/{id}/detail")]
    public async Task<IActionResult> GetMovieDetail([FromRoute] int id)
    {
        var result = await _tmdbService.GetMovieDetailAsync(id);
        if (result == null)
            return NotFound($"Filme com ID {id} não encontrado");

        return Ok(result);
    }

    [HttpGet("series/{id}/detail")]
    public async Task<IActionResult> GetSeriesDetail([FromRoute] int id)
    {
        var result = await _tmdbService.GetSeriesDetailAsync(id);
        if (result == null)
            return NotFound($"Série com ID {id} não encontrada");

        return Ok(result);
    }

    [HttpGet("movies/popular/card")]
    public async Task<IActionResult> GetPopularMoviesCard([FromQuery] int page = 1)
    {
        var result = await _tmdbService.GetPopularMoviesCardAsync(page);
        if (result == null)
            return NotFound("Nenhum filme popular encontrado");

        return Ok(result);
    }

    [HttpGet("series/popular/card")]
    public async Task<IActionResult> GetPopularSeriesCard([FromQuery] int page = 1)
    {
        var result = await _tmdbService.GetPopularSeriesCardAsync(page);
        if (result == null)
            return NotFound("Nenhuma série popular encontrada");

        return Ok(result);
    }

    [HttpGet("movies/top-rated/card")]
    public async Task<IActionResult> GetTopRatedMoviesCard([FromQuery] int page = 1)
    {
        var result = await _tmdbService.GetTopRatedMoviesCardAsync(page);
        if (result == null)
            return NotFound("Nenhum filme top-rated encontrado");

        return Ok(result);
    }

    [HttpGet("series/top-rated/card")]
    public async Task<IActionResult> GetTopRatedSeriesCard([FromQuery] int page = 1)
    {
        var result = await _tmdbService.GetTopRatedSeriesCardAsync(page);
        if (result == null)
            return NotFound("Nenhuma série top-rated encontrada");

        return Ok(result);
    }
}
