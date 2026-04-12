namespace OmnisReview.Models.TMDB;

public class TmdbPagedResultDto<T>
{
    public int Page { get; set; }
    public List<T> Results { get; set; } = new();
    public int TotalPages { get; set; }
    public int TotalResults { get; set; }
}
