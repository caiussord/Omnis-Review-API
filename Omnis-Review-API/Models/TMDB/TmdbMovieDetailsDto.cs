namespace OmnisReview.Models.TMDB;

public class TmdbMovieDetailsDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? OriginalTitle { get; set; }
    public string? Overview { get; set; }
    public double VoteAverage { get; set; }
    public int VoteCount { get; set; }
    public string? PosterPath { get; set; }
    public string? BackdropPath { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public int? Runtime { get; set; }
    public List<TmdbGenreDto> Genres { get; set; } = new();
    public double Popularity { get; set; }
    public List<TmdbCastDto> Cast { get; set; } = new();
    public List<TmdbVideoDto> Videos { get; set; } = new();
}
