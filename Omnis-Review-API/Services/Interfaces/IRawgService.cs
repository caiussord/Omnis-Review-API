using OmnisReview.Models.RAWG;

namespace OmnisReview.Services.Interfaces;

public interface IRawgService
{
    Task<RawgPagedResultDto?> SearchGamesAsync(string query, int page = 1, int pageSize = 20);
    Task<RawgPagedResultDto?> SearchGamesByGenreAsync(string genre, int page = 1, int pageSize = 20);
    Task<RawgPagedResultDto?> SearchGamesByPlatformAsync(string platform, int page = 1, int pageSize = 20);
    Task<RawgPagedResultDto?> GetPopularGamesAsync(int page = 1, int pageSize = 20);
    Task<RawgPagedResultDto?> GetUpcomingGamesAsync(int page = 1, int pageSize = 20);
    Task<RawgGameDetailDto?> GetGameByIdAsync(int gameId);
    Task<RawgPagedResultDto?> GetGamesBySortAsync(string sortBy, int page = 1, int pageSize = 20);
    Task<RawgPagedResultsDto<RawgDeveloperDto>?> GetDevelopersAsync(int page = 1, int pageSize = 20);
    Task<RawgDeveloperDetailDto?> GetDeveloperByIdAsync(int developerId);
    Task<RawgPagedResultsDto<RawgPublisherDto>?> GetPublishersAsync(int page = 1, int pageSize = 20);
    Task<RawgPublisherDetailDto?> GetPublisherByIdAsync(int publisherId);

    Task<RawgPagedResultDto?> SearchGamesCardAsync(string query, int page = 1, int pageSize = 20);
    Task<RawgPagedResultDto?> GetPopularGamesCardAsync(int page = 1, int pageSize = 20);
    Task<RawgGameDetailCardDto?> GetGameDetailAsync(int gameId);
}
