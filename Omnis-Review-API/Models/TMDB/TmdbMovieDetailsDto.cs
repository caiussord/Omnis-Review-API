using System.Text.Json.Serialization;

namespace OmnisReview.Models.TMDB;

public class TmdbMovieDetailsDto
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
    public int? Runtime { get; set; }

    [JsonPropertyName("genres")]
    public List<TmdbGenreDto> Genres { get; set; } = new();

    [JsonPropertyName("popularity")]
    public double Popularity { get; set; }

    [JsonPropertyName("credits")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TmdbCreditsDto? Credits { get; set; }
}

public class TmdbCreditsDto
{
    [JsonPropertyName("cast")]
    public List<TmdbCastDto> Cast { get; set; } = new();

    [JsonPropertyName("crew")]
    public List<object> Crew { get; set; } = new();
}

public class TmdbMovieDetailsInternalDto
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
