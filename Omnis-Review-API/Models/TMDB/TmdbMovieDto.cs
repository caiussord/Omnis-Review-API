using System.Text.Json.Serialization;

namespace OmnisReview.Models.TMDB;

public class TmdbMovieDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("original_title")]
    public string? OriginalTitle { get; set; }

    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }

    [JsonPropertyName("vote_count")]
    public int VoteCount { get; set; }

    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }

    [JsonPropertyName("backdrop_path")]
    public string? BackdropPath { get; set; }

    [JsonPropertyName("release_date")]
    public DateTime? ReleaseDate { get; set; }

    [JsonPropertyName("runtime")]
    public int Runtime { get; set; }

    [JsonPropertyName("genre_ids")]
    public List<int> GenreIds { get; set; } = new();

    [JsonPropertyName("popularity")]
    public double Popularity { get; set; }

    [JsonPropertyName("video")]
    public bool Video { get; set; }

    [JsonPropertyName("adult")]
    public bool Adult { get; set; }

    [JsonPropertyName("original_language")]
    public string? OriginalLanguage { get; set; }
}
