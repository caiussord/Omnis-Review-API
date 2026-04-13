using OmnisReview.Models.TMDB;

namespace OmnisReview.Services.Interfaces;

public interface ITmdbService
{
    Task<TmdbPagedResultDto<TmdbMovieDto>?> SearchMoviesAsync(string query, int page = 1, string language = "pt-BR");
    Task<TmdbPagedResultDto<TmdbSeriesDto>?> SearchSeriesAsync(string query, int page = 1, string language = "pt-BR");
    Task<TmdbMovieDetailsDto?> GetMovieDetailsAsync(int movieId, string language = "pt-BR");
    Task<TmdbSeriesDetailsDto?> GetSeriesDetailsAsync(int seriesId, string language = "pt-BR");
    Task<List<TmdbCastDto>?> GetMovieCastAsync(int movieId, string language = "pt-BR");
    Task<List<TmdbCastDto>?> GetSeriesCastAsync(int seriesId, string language = "pt-BR");
    Task<List<TmdbVideoDto>?> GetMovieVideosAsync(int movieId, string language = "pt-BR");
    Task<List<TmdbVideoDto>?> GetSeriesVideosAsync(int seriesId, string language = "pt-BR");
    Task<TmdbPagedResultDto<TmdbMovieDto>?> GetPopularMoviesAsync(int page = 1, string language = "pt-BR");
    Task<TmdbPagedResultDto<TmdbSeriesDto>?> GetPopularSeriesAsync(int page = 1, string language = "pt-BR");
    Task<TmdbPagedResultDto<TmdbMovieDto>?> GetTopRatedMoviesAsync(int page = 1, string language = "pt-BR");
    Task<TmdbPagedResultDto<TmdbSeriesDto>?> GetTopRatedSeriesAsync(int page = 1, string language = "pt-BR");
    Task<TmdbSeasonDto?> GetSeasonAsync(int seriesId, int seasonNumber, string language = "pt-BR");
    Task<TmdbEpisodeDto?> GetEpisodeAsync(int seriesId, int seasonNumber, int episodeNumber, string language = "pt-BR");
}
