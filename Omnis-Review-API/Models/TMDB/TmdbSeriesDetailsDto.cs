using System.Text.Json.Serialization;

namespace OmnisReview.Models.TMDB;

public class TmdbSeriesDetailsDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("original_name")]
    public string? OriginalName { get; set; }

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

    [JsonPropertyName("first_air_date")]
    public DateTime? FirstAirDate { get; set; }

    [JsonPropertyName("last_air_date")]
    public DateTime? LastAirDate { get; set; }

    [JsonPropertyName("number_of_seasons")]
    public int NumberOfSeasons { get; set; }

    [JsonPropertyName("number_of_episodes")]
    public int NumberOfEpisodes { get; set; }

    [JsonPropertyName("genres")]
    public List<TmdbGenreDto> Genres { get; set; } = new();

    [JsonPropertyName("popularity")]
    public double Popularity { get; set; }

    [JsonPropertyName("credits")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TmdbCreditsDto? Credits { get; set; }
}

public class TmdbSeriesDetailsInternalDto
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
    public List<TmdbGenreDto> Genres { get; set; } = new();
    public double Popularity { get; set; }
    public List<TmdbCastDto> Cast { get; set; } = new();
    public List<TmdbVideoDto> Videos { get; set; } = new();
}
