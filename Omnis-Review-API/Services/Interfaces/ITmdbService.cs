using OmnisReview.Models.TMDB;

namespace OmnisReview.Services.Interfaces;

public interface ITmdbService
{
    Task<TmdbPagedResultDto<TmdbMovieDto>?> SearchMoviesAsync(string query, int page = 1, string language = "en-US");
    Task<TmdbPagedResultDto<TmdbSeriesDto>?> SearchSeriesAsync(string query, int page = 1, string language = "en-US");
    Task<TmdbMovieDetailsDto?> GetMovieDetailsAsync(int movieId, string language = "en-US");
    Task<TmdbSeriesDetailsDto?> GetSeriesDetailsAsync(int seriesId, string language = "en-US");
    Task<List<TmdbCastDto>?> GetMovieCastAsync(int movieId);
    Task<List<TmdbCastDto>?> GetSeriesCastAsync(int seriesId);
    Task<List<TmdbVideoDto>?> GetMovieVideosAsync(int movieId);
    Task<List<TmdbVideoDto>?> GetSeriesVideosAsync(int seriesId);
    Task<TmdbPagedResultDto<TmdbMovieDto>?> GetPopularMoviesAsync(int page = 1, string language = "en-US");
    Task<TmdbPagedResultDto<TmdbSeriesDto>?> GetPopularSeriesAsync(int page = 1, string language = "en-US");
    Task<TmdbPagedResultDto<TmdbMovieDto>?> GetTopRatedMoviesAsync(int page = 1, string language = "en-US");
    Task<TmdbPagedResultDto<TmdbSeriesDto>?> GetTopRatedSeriesAsync(int page = 1, string language = "en-US");
}
