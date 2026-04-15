using System.Text.Json;
using OmnisReview.Models.GoogleBooks;
using OmnisReview.Services.Interfaces;

namespace OmnisReview.Services;

public class GoogleBooksService : IGoogleBooksService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<GoogleBooksService> _logger;
    private readonly string _apiKey;
    private const string BaseUrl = "https://www.googleapis.com/books/v1";

    public GoogleBooksService(HttpClient httpClient, IConfiguration configuration, ILogger<GoogleBooksService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
        _apiKey = _configuration["GoogleBooks:ApiKey"] ?? throw new InvalidOperationException("Google Books API key not configured");
    }

    public async Task<GoogleBooksPagedResultDto?> SearchBooksAsync(string query, int startIndex = 0, int maxResults = 10)
    {
        var url = $"{BaseUrl}/volumes?q={Uri.EscapeDataString(query)}&startIndex={startIndex}&maxResults={maxResults}&key={_apiKey}";
        return await GetAsync<GoogleBooksPagedResultDto>(url);
    }

    public async Task<GoogleBooksPagedResultDto?> SearchByTitleAsync(string title, int startIndex = 0, int maxResults = 10)
    {
        var query = $"intitle:{Uri.EscapeDataString(title)}";
        var url = $"{BaseUrl}/volumes?q={query}&startIndex={startIndex}&maxResults={maxResults}&key={_apiKey}";
        return await GetAsync<GoogleBooksPagedResultDto>(url);
    }

    public async Task<GoogleBooksPagedResultDto?> SearchByAuthorAsync(string author, int startIndex = 0, int maxResults = 10)
    {
        var query = $"inauthor:{Uri.EscapeDataString(author)}";
        var url = $"{BaseUrl}/volumes?q={query}&startIndex={startIndex}&maxResults={maxResults}&key={_apiKey}";
        return await GetAsync<GoogleBooksPagedResultDto>(url);
    }

    public async Task<GoogleBooksPagedResultDto?> SearchByISBNAsync(string isbn)
    {
        var query = $"isbn:{Uri.EscapeDataString(isbn)}";
        var url = $"{BaseUrl}/volumes?q={query}&maxResults=1&key={_apiKey}";
        return await GetAsync<GoogleBooksPagedResultDto>(url);
    }

    public async Task<GoogleBooksPagedResultDto?> SearchByPublisherAsync(string publisher, int startIndex = 0, int maxResults = 10)
    {
        var query = $"inpublisher:{Uri.EscapeDataString(publisher)}";
        var url = $"{BaseUrl}/volumes?q={query}&startIndex={startIndex}&maxResults={maxResults}&key={_apiKey}";
        return await GetAsync<GoogleBooksPagedResultDto>(url);
    }

    public async Task<GoogleBooksBookDto?> GetBookByIdAsync(string bookId)
    {
        var url = $"{BaseUrl}/volumes/{bookId}?key={_apiKey}";
        return await GetAsync<GoogleBooksBookDto>(url);
    }

    public async Task<List<BookCardDto>?> SearchBooksCardAsync(string query, int startIndex = 0, int maxResults = 10)
    {
        var result = await SearchBooksAsync(query, startIndex, maxResults);
        if (result?.Items == null) return null;

        var cardResults = new List<BookCardDto>();
        foreach (var item in result.Items)
        {
            var volumeInfo = item.VolumeInfo;
            if (volumeInfo != null)
            {
                cardResults.Add(new BookCardDto
                {
                    Id = item.Id,
                    Title = volumeInfo.Title,
                    AverageRating = volumeInfo.AverageRating,
                    Categories = volumeInfo.Categories,
                    CoverImage = volumeInfo.ImageLinks?.Thumbnail
                });
            }
        }

        return cardResults;
    }

    public async Task<BookDetailDto?> GetBookDetailAsync(string bookId)
    {
        var book = await GetBookByIdAsync(bookId);
        if (book?.VolumeInfo == null) return null;

        var volumeInfo = book.VolumeInfo;
        return new BookDetailDto
        {
            Id = book.Id,
            Title = volumeInfo.Title,
            AverageRating = volumeInfo.AverageRating,
            Categories = volumeInfo.Categories,
            CoverImage = volumeInfo.ImageLinks?.Thumbnail,
            Authors = volumeInfo.Authors,
            Publisher = volumeInfo.Publisher,
            PublishedDate = volumeInfo.PublishedDate,
            Description = volumeInfo.Description,
            PageCount = volumeInfo.PageCount
        };
    }

    public async Task<List<BookCardDto>?> GetBestsellerBooksCardAsync(int page = 0)
    {
        var startIndex = page * 10;
        var query = "subject:bestseller";
        var url = $"{BaseUrl}/volumes?q={Uri.EscapeDataString(query)}&orderBy=relevance&startIndex={startIndex}&maxResults=10&key={_apiKey}";

        var result = await GetAsync<GoogleBooksPagedResultDto>(url);
        if (result?.Items is null) return null;

        return result.Items
            .Select(item => new BookCardDto
            {
                Id = item.Id,
                Title = item.VolumeInfo?.Title ?? string.Empty,
                CoverImage = item.VolumeInfo?.ImageLinks?.Thumbnail,
                AverageRating = item.VolumeInfo?.AverageRating ?? 0
            })
            .ToList();
    }

    public async Task<List<BookCardDto>?> GetBooksByGenreCardAsync(string genre, int page = 0)
    {
        if (string.IsNullOrWhiteSpace(genre))
            return null;

        var startIndex = page * 10;
        var query = $"subject:{Uri.EscapeDataString(genre)}";
        var url = $"{BaseUrl}/volumes?q={query}&startIndex={startIndex}&maxResults=10&key={_apiKey}";

        var result = await GetAsync<GoogleBooksPagedResultDto>(url);
        if (result?.Items is null) return null;

        return result.Items
            .Select(item => new BookCardDto
            {
                Id = item.Id,
                Title = item.VolumeInfo?.Title ?? string.Empty,
                CoverImage = item.VolumeInfo?.ImageLinks?.Thumbnail,
                AverageRating = item.VolumeInfo?.AverageRating ?? 0
            })
            .ToList();
    }

    private async Task<T?> GetAsync<T>(string url)
    {
        try
        {
            _logger.LogInformation("Requesting: {Url}", url);
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("API returned status code {StatusCode}", response.StatusCode);
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Error response: {ErrorContent}", errorContent);
                return default;
            }

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Successfully retrieved content, length: {ContentLength}", content.Length);
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during API request");
            return default;
        }
    }
}
