using System.Text.Json;
using OmnisReview.Models.TMDB;
using OmnisReview.Services.Interfaces;

namespace OmnisReview.Services;

public class TmdbService : ITmdbService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly string _apiKey;
    private const string BaseUrl = "https://api.themoviedb.org/3";
    private const string ImageBaseUrl = "https://image.tmdb.org/t/p";

    public TmdbService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _apiKey = _configuration["Tmdb:ApiKey"] ?? throw new InvalidOperationException("TMDB API key not configured");
    }

    public async Task<TmdbPagedResultDto<TmdbMovieDto>?> SearchMoviesAsync(string query, int page = 1, string language = "pt-BR")
    {
        var url = $"{BaseUrl}/search/movie?api_key={_apiKey}&query={Uri.EscapeDataString(query)}&page={page}&language={language}";
        return await GetAsync<TmdbPagedResultDto<TmdbMovieDto>>(url);
    }

    public async Task<TmdbPagedResultDto<TmdbSeriesDto>?> SearchSeriesAsync(string query, int page = 1, string language = "pt-BR")
    {
        var url = $"{BaseUrl}/search/tv?api_key={_apiKey}&query={Uri.EscapeDataString(query)}&page={page}&language={language}";
        return await GetAsync<TmdbPagedResultDto<TmdbSeriesDto>>(url);
    }

    public async Task<TmdbMovieDetailsDto?> GetMovieDetailsAsync(int movieId, string language = "pt-BR")
    {
        var url = $"{BaseUrl}/movie/{movieId}?api_key={_apiKey}&language={language}&append_to_response=credits,videos";
        return await GetAsync<TmdbMovieDetailsDto>(url);
    }

    public async Task<TmdbSeriesDetailsDto?> GetSeriesDetailsAsync(int seriesId, string language = "pt-BR")
    {
        var url = $"{BaseUrl}/tv/{seriesId}?api_key={_apiKey}&language={language}&append_to_response=credits,videos";
        return await GetAsync<TmdbSeriesDetailsDto>(url);
    }

    public async Task<List<TmdbCastDto>?> GetMovieCastAsync(int movieId, string language = "pt-BR")
    {
        var url = $"{BaseUrl}/movie/{movieId}/credits?api_key={_apiKey}&language={language}";
        var response = await GetAsync<dynamic>(url);
        
        if (response is null)
            return null;

        var cast = new List<TmdbCastDto>();
        var castArray = response?["cast"] as JsonElement?;
        
        if (castArray.HasValue)
        {
            foreach (var item in castArray.Value.EnumerateArray().Take(20))
            {
                cast.Add(new TmdbCastDto
                {
                    Id = item.GetProperty("id").GetInt32(),
                    Name = item.GetProperty("name").GetString(),
                    Character = item.GetProperty("character").GetString(),
                    ProfilePath = item.GetProperty("profile_path").GetString(),
                    Order = item.GetProperty("order").GetInt32()
                });
            }
        }

        return cast;
    }

    public async Task<List<TmdbCastDto>?> GetSeriesCastAsync(int seriesId, string language = "pt-BR")
    {
        var url = $"{BaseUrl}/tv/{seriesId}/credits?api_key={_apiKey}&language={language}";
        var response = await GetAsync<dynamic>(url);
        
        if (response is null)
            return null;

        var cast = new List<TmdbCastDto>();
        var castArray = response?["cast"] as JsonElement?;
        
        if (castArray.HasValue)
        {
            foreach (var item in castArray.Value.EnumerateArray().Take(20))
            {
                cast.Add(new TmdbCastDto
                {
                    Id = item.GetProperty("id").GetInt32(),
                    Name = item.GetProperty("name").GetString(),
                    Character = item.GetProperty("character").GetString(),
                    ProfilePath = item.GetProperty("profile_path").GetString(),
                    Order = item.GetProperty("order").GetInt32()
                });
            }
        }

        return cast;
    }

    public async Task<List<TmdbVideoDto>?> GetMovieVideosAsync(int movieId, string language = "pt-BR")
    {
        var url = $"{BaseUrl}/movie/{movieId}/videos?api_key={_apiKey}&language={language}";
        var response = await GetAsync<dynamic>(url);
        
        if (response is null)
            return null;

        var videos = new List<TmdbVideoDto>();
        var resultsArray = response?["results"] as JsonElement?;
        
        if (resultsArray.HasValue)
        {
            foreach (var item in resultsArray.Value.EnumerateArray().Where(v => v.GetProperty("site").GetString() == "YouTube"))
            {
                videos.Add(new TmdbVideoDto
                {
                    Id = item.GetProperty("id").GetString(),
                    Name = item.GetProperty("name").GetString(),
                    Key = item.GetProperty("key").GetString(),
                    Site = item.GetProperty("site").GetString(),
                    Type = item.GetProperty("type").GetString()
                });
            }
        }

        return videos;
    }

    public async Task<List<TmdbVideoDto>?> GetSeriesVideosAsync(int seriesId, string language = "pt-BR")
    {
        var url = $"{BaseUrl}/tv/{seriesId}/videos?api_key={_apiKey}&language={language}";
        var response = await GetAsync<dynamic>(url);
        
        if (response is null)
            return null;

        var videos = new List<TmdbVideoDto>();
        var resultsArray = response?["results"] as JsonElement?;
        
        if (resultsArray.HasValue)
        {
            foreach (var item in resultsArray.Value.EnumerateArray().Where(v => v.GetProperty("site").GetString() == "YouTube"))
            {
                videos.Add(new TmdbVideoDto
                {
                    Id = item.GetProperty("id").GetString(),
                    Name = item.GetProperty("name").GetString(),
                    Key = item.GetProperty("key").GetString(),
                    Site = item.GetProperty("site").GetString(),
                    Type = item.GetProperty("type").GetString()
                });
            }
        }

        return videos;
    }

    public async Task<TmdbPagedResultDto<TmdbMovieDto>?> GetPopularMoviesAsync(int page = 1, string language = "pt-BR")
    {
        var url = $"{BaseUrl}/movie/popular?api_key={_apiKey}&page={page}&language={language}";
        return await GetAsync<TmdbPagedResultDto<TmdbMovieDto>>(url);
    }

    public async Task<TmdbPagedResultDto<TmdbSeriesDto>?> GetPopularSeriesAsync(int page = 1, string language = "pt-BR")
    {
        var url = $"{BaseUrl}/tv/popular?api_key={_apiKey}&page={page}&language={language}";
        return await GetAsync<TmdbPagedResultDto<TmdbSeriesDto>>(url);
    }

    public async Task<TmdbPagedResultDto<TmdbMovieDto>?> GetTopRatedMoviesAsync(int page = 1, string language = "pt-BR")
    {
        var url = $"{BaseUrl}/movie/top_rated?api_key={_apiKey}&page={page}&language={language}";
        return await GetAsync<TmdbPagedResultDto<TmdbMovieDto>>(url);
    }

    public async Task<TmdbPagedResultDto<TmdbSeriesDto>?> GetTopRatedSeriesAsync(int page = 1, string language = "pt-BR")
    {
        var url = $"{BaseUrl}/tv/top_rated?api_key={_apiKey}&page={page}&language={language}";
        return await GetAsync<TmdbPagedResultDto<TmdbSeriesDto>>(url);
    }

    private async Task<T?> GetAsync<T>(string url)
    {
        try
        {
            var response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
                return default;

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception)
        {
            return default;
        }
    }
}
