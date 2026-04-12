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
