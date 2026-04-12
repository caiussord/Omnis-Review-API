namespace OmnisReview.Models.TMDB;

public class TmdbSeriesDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? OriginalName { get; set; }
    public string? Overview { get; set; }
    public double VoteAverage { get; set; }
    public int VoteCount { get; set; }
    public string? PosterPath { get; set; }
    public string? BackdropPath { get; set; }
    public DateTime? FirstAirDate { get; set; }
    public DateTime? LastAirDate { get; set; }
    public int NumberOfSeasons { get; set; }
    public int NumberOfEpisodes { get; set; }
    public List<int> GenreIds { get; set; } = new();
    public double Popularity { get; set; }
}
