using Microsoft.AspNetCore.Mvc;
using OmnisReview.Services.Interfaces;

namespace OmnisReview.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GoogleBooksController : ControllerBase
{
    private readonly IGoogleBooksService _googleBooksService;

    public GoogleBooksController(IGoogleBooksService googleBooksService)
    {
        _googleBooksService = googleBooksService;
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchBooks([FromQuery] string query, [FromQuery] int startIndex = 0, [FromQuery] int maxResults = 10)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query parameter is required");

        if (maxResults > 40)
            maxResults = 40;

        var result = await _googleBooksService.SearchBooksAsync(query, startIndex, maxResults);
        return Ok(result);
    }

    [HttpGet("search/title")]
    public async Task<IActionResult> SearchByTitle([FromQuery] string title, [FromQuery] int startIndex = 0, [FromQuery] int maxResults = 10)
    {
        if (string.IsNullOrWhiteSpace(title))
            return BadRequest("Title parameter is required");

        if (maxResults > 40)
            maxResults = 40;

        var result = await _googleBooksService.SearchByTitleAsync(title, startIndex, maxResults);
        return Ok(result);
    }

    [HttpGet("search/author")]
    public async Task<IActionResult> SearchByAuthor([FromQuery] string author, [FromQuery] int startIndex = 0, [FromQuery] int maxResults = 10)
    {
        if (string.IsNullOrWhiteSpace(author))
            return BadRequest("Author parameter is required");

        if (maxResults > 40)
            maxResults = 40;

        var result = await _googleBooksService.SearchByAuthorAsync(author, startIndex, maxResults);
        return Ok(result);
    }

    [HttpGet("search/isbn")]
    public async Task<IActionResult> SearchByISBN([FromQuery] string isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn))
            return BadRequest("ISBN parameter is required");

        var result = await _googleBooksService.SearchByISBNAsync(isbn);
        return Ok(result);
    }

    [HttpGet("search/publisher")]
    public async Task<IActionResult> SearchByPublisher([FromQuery] string publisher, [FromQuery] int startIndex = 0, [FromQuery] int maxResults = 10)
    {
        if (string.IsNullOrWhiteSpace(publisher))
            return BadRequest("Publisher parameter is required");

        if (maxResults > 40)
            maxResults = 40;

        var result = await _googleBooksService.SearchByPublisherAsync(publisher, startIndex, maxResults);
        return Ok(result);
    }

    [HttpGet("{bookId}")]
    public async Task<IActionResult> GetBook([FromRoute] string bookId)
    {
        if (string.IsNullOrWhiteSpace(bookId))
            return BadRequest("Book ID is required");

        var result = await _googleBooksService.GetBookByIdAsync(bookId);

        if (result is null)
            return NotFound("Book not found");

        return Ok(result);
    }

    [HttpGet("search/card")]
    public async Task<IActionResult> SearchBooksCard([FromQuery] string query, [FromQuery] int startIndex = 0, [FromQuery] int maxResults = 10)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query parameter is required");

        if (maxResults > 40)
            maxResults = 40;

        var result = await _googleBooksService.SearchBooksCardAsync(query, startIndex, maxResults);
        if (result == null || result.Count == 0)
            return NotFound("Nenhum livro encontrado");

        return Ok(result);
    }

    [HttpGet("{bookId}/detail")]
    public async Task<IActionResult> GetBookDetail([FromRoute] string bookId)
    {
        if (string.IsNullOrWhiteSpace(bookId))
            return BadRequest("Book ID is required");

        var result = await _googleBooksService.GetBookDetailAsync(bookId);
        if (result == null)
            return NotFound("Livro não encontrado");

        return Ok(result);
    }

    [HttpGet("bestsellers/card")]
    public async Task<IActionResult> GetBestsellerBooksCard([FromQuery] int page = 0)
    {
        try
        {
            if (page < 0)
                return BadRequest("Page must be greater than or equal to 0");

            var result = await _googleBooksService.GetBestsellerBooksCardAsync(page);
            if (result == null || result.Count == 0)
                return NotFound("Nenhum bestseller encontrado");

            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro ao recuperar bestsellers");
        }
    }

    [HttpGet("genre/{genre}/card")]
    public async Task<IActionResult> GetBooksByGenreCard([FromRoute] string genre, [FromQuery] int page = 0)
    {
        if (string.IsNullOrWhiteSpace(genre))
            return BadRequest("Genre parameter is required");

        try
        {
            if (page < 0)
                return BadRequest("Page must be greater than or equal to 0");

            var result = await _googleBooksService.GetBooksByGenreCardAsync(genre, page);
            if (result == null || result.Count == 0)
                return NotFound($"Nenhum livro encontrado para o gênero: {genre}");

            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro ao recuperar livros do gênero");
        }
    }
}
