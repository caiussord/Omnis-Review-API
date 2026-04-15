namespace OmnisReview.Models.TMDB;

public class TmdbSeriesCardDto
{
    public int Id { get; set; }
    public string? PosterPath { get; set; }
    public string? Name { get; set; }
    public double VoteAverage { get; set; }
    public List<int> GenreIds { get; set; } = new();
    public MediaType Type => MediaType.Series;
}
