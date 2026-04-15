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
        var response = await GetAsync<JsonElement?>(url);

        if (!response.HasValue)
            return null;

        var cast = new List<TmdbCastDto>();

        try
        {
            if (response.Value.TryGetProperty("cast", out var castArray))
            {
                foreach (var item in castArray.EnumerateArray().Take(20))
                {
                    cast.Add(new TmdbCastDto
                    {
                        Id = item.GetProperty("id").GetInt32(),
                        Name = item.GetProperty("name").GetString() ?? string.Empty,
                        Character = item.GetProperty("character").GetString() ?? string.Empty,
                        ProfilePath = item.GetProperty("profile_path").GetString(),
                        Order = item.GetProperty("order").GetInt32()
                    });
                }
            }
        }
        catch (Exception)
        {
            return null;
        }

        return cast;
    }

    public async Task<List<TmdbCastDto>?> GetSeriesCastAsync(int seriesId, string language = "pt-BR")
    {
        var url = $"{BaseUrl}/tv/{seriesId}/credits?api_key={_apiKey}&language={language}";
        var response = await GetAsync<JsonElement?>(url);

        if (!response.HasValue)
            return null;

        var cast = new List<TmdbCastDto>();

        try
        {
            if (response.Value.TryGetProperty("cast", out var castArray))
            {
                foreach (var item in castArray.EnumerateArray().Take(20))
                {
                    cast.Add(new TmdbCastDto
                    {
                        Id = item.GetProperty("id").GetInt32(),
                        Name = item.GetProperty("name").GetString() ?? string.Empty,
                        Character = item.GetProperty("character").GetString() ?? string.Empty,
                        ProfilePath = item.GetProperty("profile_path").GetString(),
                        Order = item.GetProperty("order").GetInt32()
                    });
                }
            }
        }
        catch (Exception)
        {
            return null;
        }

        return cast;
    }

    public async Task<List<TmdbVideoDto>?> GetMovieVideosAsync(int movieId, string language = "pt-BR")
    {
        var url = $"{BaseUrl}/movie/{movieId}/videos?api_key={_apiKey}&language={language}";
        var response = await GetAsync<JsonElement?>(url);

        if (!response.HasValue)
            return null;

        var videos = new List<TmdbVideoDto>();

        try
        {
            if (response.Value.TryGetProperty("results", out var resultsArray))
            {
                foreach (var item in resultsArray.EnumerateArray().Where(v => v.TryGetProperty("site", out var site) && site.GetString() == "YouTube"))
                {
                    videos.Add(new TmdbVideoDto
                    {
                        Id = item.GetProperty("id").GetString() ?? string.Empty,
                        Name = item.GetProperty("name").GetString() ?? string.Empty,
                        Key = item.GetProperty("key").GetString() ?? string.Empty,
                        Site = item.GetProperty("site").GetString() ?? string.Empty,
                        Type = item.GetProperty("type").GetString() ?? string.Empty
                    });
                }
            }
        }
        catch (Exception)
        {
            return null;
        }

        return videos;
    }

    public async Task<List<TmdbVideoDto>?> GetSeriesVideosAsync(int seriesId, string language = "pt-BR")
    {
        var url = $"{BaseUrl}/tv/{seriesId}/videos?api_key={_apiKey}&language={language}";
        var response = await GetAsync<JsonElement?>(url);

        if (!response.HasValue)
            return null;

        var videos = new List<TmdbVideoDto>();

        try
        {
            if (response.Value.TryGetProperty("results", out var resultsArray))
            {
                foreach (var item in resultsArray.EnumerateArray().Where(v => v.TryGetProperty("site", out var site) && site.GetString() == "YouTube"))
                {
                    videos.Add(new TmdbVideoDto
                    {
                        Id = item.GetProperty("id").GetString() ?? string.Empty,
                        Name = item.GetProperty("name").GetString() ?? string.Empty,
                        Key = item.GetProperty("key").GetString() ?? string.Empty,
                        Site = item.GetProperty("site").GetString() ?? string.Empty,
                        Type = item.GetProperty("type").GetString() ?? string.Empty
                    });
                }
            }
        }
        catch (Exception)
        {
            return null;
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

    public async Task<TmdbSeasonDto?> GetSeasonAsync(int seriesId, int seasonNumber, string language = "pt-BR")
    {
        var url = $"{BaseUrl}/tv/{seriesId}/season/{seasonNumber}?api_key={_apiKey}&language={language}";
        return await GetAsync<TmdbSeasonDto>(url);
    }

    public async Task<TmdbEpisodeDto?> GetEpisodeAsync(int seriesId, int seasonNumber, int episodeNumber, string language = "pt-BR")
    {
        var url = $"{BaseUrl}/tv/{seriesId}/season/{seasonNumber}/episode/{episodeNumber}?api_key={_apiKey}&language={language}";
        return await GetAsync<TmdbEpisodeDto>(url);
    }

    public async Task<TmdbPagedResultDto<TmdbMovieCardDto>?> SearchMoviesCardAsync(string query, int page = 1, string language = "pt-BR")
    {
        var result = await SearchMoviesAsync(query, page, language);
        if (result is null) return null;

        return new TmdbPagedResultDto<TmdbMovieCardDto>
        {
            Page = result.Page,
            Results = result.Results?.Select(m => new TmdbMovieCardDto
            {
                Id = m.Id,
                PosterPath = m.PosterPath,
                Title = m.Title,
                VoteAverage = m.VoteAverage,
                GenreIds = m.GenreIds
            }).ToList() ?? new(),
            TotalPages = result.TotalPages,
            TotalResults = result.TotalResults
        };
    }

    public async Task<TmdbPagedResultDto<TmdbSeriesCardDto>?> SearchSeriesCardAsync(string query, int page = 1, string language = "pt-BR")
    {
        var result = await SearchSeriesAsync(query, page, language);
        if (result is null) return null;

        return new TmdbPagedResultDto<TmdbSeriesCardDto>
        {
            Page = result.Page,
            Results = result.Results?.Select(s => new TmdbSeriesCardDto
            {
                Id = s.Id,
                PosterPath = s.PosterPath,
                Name = s.Name,
                VoteAverage = s.VoteAverage,
                GenreIds = s.GenreIds
            }).ToList() ?? new(),
            TotalPages = result.TotalPages,
            TotalResults = result.TotalResults
        };
    }

    public async Task<TmdbMovieDetailDto?> GetMovieDetailAsync(int movieId, string language = "pt-BR")
    {
        var details = await GetMovieDetailsAsync(movieId, language);
        if (details is null) return null;

        var cast = await GetMovieCastAsync(movieId, language);

        return new TmdbMovieDetailDto
        {
            Id = details.Id,
            PosterPath = details.PosterPath,
            Title = details.Title,
            VoteAverage = details.VoteAverage,
            GenreIds = new(),
            Overview = details.Overview,
            ReleaseDate = details.ReleaseDate,
            Cast = cast ?? new(),
            Genres = details.Genres ?? new()
        };
    }

    public async Task<TmdbSeriesDetailDto?> GetSeriesDetailAsync(int seriesId, string language = "pt-BR")
    {
        var details = await GetSeriesDetailsAsync(seriesId, language);
        if (details is null) return null;

        var cast = await GetSeriesCastAsync(seriesId, language);

        return new TmdbSeriesDetailDto
        {
            Id = details.Id,
            PosterPath = details.PosterPath,
            Name = details.Name,
            VoteAverage = details.VoteAverage,
            GenreIds = new(),
            Overview = details.Overview,
            FirstAirDate = details.FirstAirDate,
            Cast = cast ?? new(),
            Genres = details.Genres ?? new()
        };
    }

    public async Task<TmdbPagedResultDto<TmdbMovieCardDto>?> GetPopularMoviesCardAsync(int page = 1, string language = "pt-BR")
    {
        var result = await GetPopularMoviesAsync(page, language);
        if (result is null) return null;

        return new TmdbPagedResultDto<TmdbMovieCardDto>
        {
            Page = result.Page,
            Results = result.Results?.Select(m => new TmdbMovieCardDto
            {
                Id = m.Id,
                PosterPath = m.PosterPath,
                Title = m.Title,
                VoteAverage = m.VoteAverage,
                GenreIds = m.GenreIds
            }).ToList() ?? new(),
            TotalPages = result.TotalPages,
            TotalResults = result.TotalResults
        };
    }

    public async Task<TmdbPagedResultDto<TmdbSeriesCardDto>?> GetPopularSeriesCardAsync(int page = 1, string language = "pt-BR")
    {
        var result = await GetPopularSeriesAsync(page, language);
        if (result is null) return null;

        return new TmdbPagedResultDto<TmdbSeriesCardDto>
        {
            Page = result.Page,
            Results = result.Results?.Select(s => new TmdbSeriesCardDto
            {
                Id = s.Id,
                PosterPath = s.PosterPath,
                Name = s.Name,
                VoteAverage = s.VoteAverage,
                GenreIds = s.GenreIds
            }).ToList() ?? new(),
            TotalPages = result.TotalPages,
            TotalResults = result.TotalResults
        };
    }

    public async Task<TmdbPagedResultDto<TmdbMovieCardDto>?> GetTopRatedMoviesCardAsync(int page = 1, string language = "pt-BR")
    {
        var result = await GetTopRatedMoviesAsync(page, language);
        if (result is null) return null;

        return new TmdbPagedResultDto<TmdbMovieCardDto>
        {
            Page = result.Page,
            Results = result.Results?.Select(m => new TmdbMovieCardDto
            {
                Id = m.Id,
                PosterPath = m.PosterPath,
                Title = m.Title,
                VoteAverage = m.VoteAverage,
                GenreIds = m.GenreIds
            }).ToList() ?? new(),
            TotalPages = result.TotalPages,
            TotalResults = result.TotalResults
        };
    }

    public async Task<TmdbPagedResultDto<TmdbSeriesCardDto>?> GetTopRatedSeriesCardAsync(int page = 1, string language = "pt-BR")
    {
        var result = await GetTopRatedSeriesAsync(page, language);
        if (result is null) return null;

        return new TmdbPagedResultDto<TmdbSeriesCardDto>
        {
            Page = result.Page,
            Results = result.Results?.Select(s => new TmdbSeriesCardDto
            {
                Id = s.Id,
                PosterPath = s.PosterPath,
                Name = s.Name,
                VoteAverage = s.VoteAverage,
                GenreIds = s.GenreIds
            }).ToList() ?? new(),
            TotalPages = result.TotalPages,
            TotalResults = result.TotalResults
        };
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
