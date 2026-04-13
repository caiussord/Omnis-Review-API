using OmnisReview.Models.RAWG;

namespace OmnisReview.Services.Interfaces;

public interface IRawgService
{
    Task<RawgPagedResultDto?> SearchGamesAsync(string query, int page = 1, int pageSize = 20);
    Task<RawgPagedResultDto?> SearchGamesByGenreAsync(string genre, int page = 1, int pageSize = 20);
    Task<RawgPagedResultDto?> SearchGamesByPlatformAsync(string platform, int page = 1, int pageSize = 20);
    Task<RawgPagedResultDto?> GetPopularGamesAsync(int page = 1, int pageSize = 20);
    Task<RawgPagedResultDto?> GetUpcomingGamesAsync(int page = 1, int pageSize = 20);
    Task<RawgGameDto?> GetGameByIdAsync(int gameId);
    Task<RawgPagedResultDto?> GetGamesBySortAsync(string sortBy, int page = 1, int pageSize = 20);
}
