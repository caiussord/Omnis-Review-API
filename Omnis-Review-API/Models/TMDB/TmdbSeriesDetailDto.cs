namespace OmnisReview.Models.TMDB;

public class TmdbSeriesDetailDto
{
    public int Id { get; set; }
    public string? PosterPath { get; set; }
    public string? Name { get; set; }
    public double VoteAverage { get; set; }
    public List<int> GenreIds { get; set; } = new();
    public MediaType Type => MediaType.Series;
    public string? Overview { get; set; }
    public DateTime? FirstAirDate { get; set; }
    public List<TmdbCastDto> Cast { get; set; } = new();
    public List<TmdbGenreDto> Genres { get; set; } = new();
}
