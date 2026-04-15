using System.Text.Json;
using OmnisReview.Models.RAWG;
using OmnisReview.Services.Interfaces;

namespace OmnisReview.Services;

public class RawgService : IRawgService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<RawgService> _logger;
    private readonly string _apiKey;
    private const string BaseUrl = "https://api.rawg.io/api";

    public RawgService(HttpClient httpClient, IConfiguration configuration, ILogger<RawgService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
        _apiKey = _configuration["Rawg:ApiKey"] ?? throw new InvalidOperationException("RAWG API key not configured");
    }

    public async Task<RawgPagedResultDto?> SearchGamesAsync(string query, int page = 1, int pageSize = 20)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            _logger.LogWarning("Search query is empty");
            return null;
        }

        var url = $"{BaseUrl}/games?search={Uri.EscapeDataString(query)}&page={page}&page_size={pageSize}&key={_apiKey}";
        return await GetAsync<RawgPagedResultDto>(url);
    }

    public async Task<RawgPagedResultDto?> SearchGamesByGenreAsync(string genre, int page = 1, int pageSize = 20)
    {
        if (string.IsNullOrWhiteSpace(genre))
        {
            _logger.LogWarning("Genre parameter is empty");
            return null;
        }

        var url = $"{BaseUrl}/games?genres={Uri.EscapeDataString(genre)}&page={page}&page_size={pageSize}&key={_apiKey}";
        return await GetAsync<RawgPagedResultDto>(url);
    }

    public async Task<RawgPagedResultDto?> SearchGamesByPlatformAsync(string platform, int page = 1, int pageSize = 20)
    {
        if (string.IsNullOrWhiteSpace(platform))
        {
            _logger.LogWarning("Platform parameter is empty");
            return null;
        }

        var url = $"{BaseUrl}/games?platforms={Uri.EscapeDataString(platform)}&page={page}&page_size={pageSize}&key={_apiKey}";
        return await GetAsync<RawgPagedResultDto>(url);
    }

    public async Task<RawgPagedResultDto?> GetPopularGamesAsync(int page = 1, int pageSize = 20)
    {
        _logger.LogInformation("Fetching popular games, page: {Page}", page);
        var url = $"{BaseUrl}/games?ordering=-rating&page={page}&page_size={pageSize}&key={_apiKey}";
        return await GetAsync<RawgPagedResultDto>(url);
    }

    public async Task<RawgPagedResultDto?> GetUpcomingGamesAsync(int page = 1, int pageSize = 20)
    {
        _logger.LogInformation("Fetching upcoming games, page: {Page}", page);
        var today = DateTime.Now.ToString("yyyy-MM-dd");
        var url = $"{BaseUrl}/games?dates={today},2099-12-31&ordering=-released&page={page}&page_size={pageSize}&key={_apiKey}";
        return await GetAsync<RawgPagedResultDto>(url);
    }

    public async Task<RawgGameDetailDto?> GetGameByIdAsync(int gameId)
    {
        _logger.LogInformation("Fetching game details for ID: {GameId}", gameId);
        var url = $"{BaseUrl}/games/{gameId}?key={_apiKey}";
        return await GetAsync<RawgGameDetailDto>(url);
    }

    public async Task<RawgPagedResultDto?> GetGamesBySortAsync(string sortBy, int page = 1, int pageSize = 20)
    {
        // Valid sort options: -released, -added, -created, -updated, rating, -rating, relevance, -relevance
        if (string.IsNullOrWhiteSpace(sortBy))
        {
            _logger.LogWarning("Sort parameter is empty");
            return null;
        }

        _logger.LogInformation("Fetching games sorted by: {SortBy}, page: {Page}", sortBy, page);
        var url = $"{BaseUrl}/games?ordering={Uri.EscapeDataString(sortBy)}&page={page}&page_size={pageSize}&key={_apiKey}";
        return await GetAsync<RawgPagedResultDto>(url);
    }

    public async Task<RawgPagedResultsDto<RawgDeveloperDto>?> GetDevelopersAsync(int page = 1, int pageSize = 20)
    {
        _logger.LogInformation("Fetching developers, page: {Page}", page);
        var url = $"{BaseUrl}/developers?page={page}&page_size={pageSize}&key={_apiKey}";
        return await GetAsync<RawgPagedResultsDto<RawgDeveloperDto>>(url);
    }

    public async Task<RawgDeveloperDetailDto?> GetDeveloperByIdAsync(int developerId)
    {
        _logger.LogInformation("Fetching developer details for ID: {DeveloperId}", developerId);
        var url = $"{BaseUrl}/developers/{developerId}?key={_apiKey}";
        return await GetAsync<RawgDeveloperDetailDto>(url);
    }

    public async Task<RawgPagedResultsDto<RawgPublisherDto>?> GetPublishersAsync(int page = 1, int pageSize = 20)
    {
        _logger.LogInformation("Fetching publishers, page: {Page}", page);
        var url = $"{BaseUrl}/publishers?page={page}&page_size={pageSize}&key={_apiKey}";
        return await GetAsync<RawgPagedResultsDto<RawgPublisherDto>>(url);
    }

    public async Task<RawgPublisherDetailDto?> GetPublisherByIdAsync(int publisherId)
    {
        _logger.LogInformation("Fetching publisher details for ID: {PublisherId}", publisherId);
        var url = $"{BaseUrl}/publishers/{publisherId}?key={_apiKey}";
        return await GetAsync<RawgPublisherDetailDto>(url);
    }

    public async Task<RawgPagedResultDto?> SearchGamesCardAsync(string query, int page = 1, int pageSize = 20)
    {
        var result = await SearchGamesAsync(query, page, pageSize);
        if (result?.Results == null) return null;

        var cardResults = result.Results
            .Select(game => new RawgGameDto
            {
                Id = game.Id,
                Name = game.Name,
                BackgroundImage = game.BackgroundImage,
                Rating = game.Rating,
                Genres = game.Genres
            })
            .ToList();

        return new RawgPagedResultDto
        {
            Count = result.Count,
            Next = result.Next,
            Previous = result.Previous,
            Results = cardResults
        };
    }

    public async Task<RawgPagedResultDto?> GetPopularGamesCardAsync(int page = 1, int pageSize = 20)
    {
        var result = await GetPopularGamesAsync(page, pageSize);
        if (result?.Results == null) return null;

        var cardResults = result.Results
            .Select(game => new RawgGameDto
            {
                Id = game.Id,
                Name = game.Name,
                BackgroundImage = game.BackgroundImage,
                Rating = game.Rating,
                Genres = game.Genres
            })
            .ToList();

        return new RawgPagedResultDto
        {
            Count = result.Count,
            Next = result.Next,
            Previous = result.Previous,
            Results = cardResults
        };
    }

    public async Task<RawgGameDetailCardDto?> GetGameDetailAsync(int gameId)
    {
        var detail = await GetGameByIdAsync(gameId);
        if (detail is null) return null;

        return new RawgGameDetailCardDto
        {
            Id = detail.Id,
            BackgroundImage = detail.BackgroundImage,
            Name = detail.Name,
            Rating = detail.Rating,
            Genres = detail.Genres,
            Description = detail.Description,
            Released = detail.Released,
            Developers = detail.Developers,
            Publishers = detail.Publishers
        };
    }

    private async Task<T?> GetAsync<T>(string url) where T : class
    {
        try
        {
            // Log mascarando a API key
            var urlWithoutKey = url;
            if (url.Contains("key="))
            {
                var ampKeyIndex = url.LastIndexOf("&key=");
                var questionKeyIndex = url.LastIndexOf("?key=");

                if (ampKeyIndex > -1)
                {
                    // API key is not the first parameter
                    urlWithoutKey = url.Substring(0, ampKeyIndex) + "&key=***";
                }
                else if (questionKeyIndex > -1)
                {
                    // API key is the first/only parameter
                    urlWithoutKey = url.Substring(0, questionKeyIndex) + "?key=***";
                }
            }

            _logger.LogDebug("Calling RAWG API: {Url}", urlWithoutKey);
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("RAWG API error: {StatusCode} - {ReasonPhrase}", response.StatusCode, response.ReasonPhrase);
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();

            // Log do conteúdo recebido (primeiros 500 caracteres)
            var logContent = content.Length > 500 ? content.Substring(0, 500) + "..." : content;
            _logger.LogDebug("RAWG API response: {Content}", logContent);

            if (string.IsNullOrWhiteSpace(content))
            {
                _logger.LogWarning("RAWG API returned empty response");
                return null;
            }

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<T>(content, options);

            if (result == null)
            {
                _logger.LogWarning("Deserialization resulted in null for type {Type}", typeof(T).Name);
            }

            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError("HTTP request error: {Message}", ex.Message);
            return null;
        }
        catch (JsonException ex)
        {
            _logger.LogError("JSON deserialization error: {Message} at {Path}", ex.Message, ex.Path);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetAsync");
            return null;
        }
    }
}
