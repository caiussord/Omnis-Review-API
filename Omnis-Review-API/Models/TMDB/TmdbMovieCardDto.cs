namespace OmnisReview.Models.TMDB;

public class TmdbMovieCardDto
{
    public int Id { get; set; }
    public string? PosterPath { get; set; }
    public string? Title { get; set; }
    public double VoteAverage { get; set; }
    public List<int> GenreIds { get; set; } = new();
    public MediaType Type => MediaType.Movie;
}
