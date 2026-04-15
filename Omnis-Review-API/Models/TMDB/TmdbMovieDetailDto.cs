namespace OmnisReview.Models.TMDB;

public class TmdbMovieDetailDto
{
    public int Id { get; set; }
    public string? PosterPath { get; set; }
    public string? Title { get; set; }
    public double VoteAverage { get; set; }
    public List<int> GenreIds { get; set; } = new();
    public MediaType Type => MediaType.Movie;
    public string? Overview { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public List<TmdbCastDto> Cast { get; set; } = new();
    public List<TmdbGenreDto> Genres { get; set; } = new();
}
