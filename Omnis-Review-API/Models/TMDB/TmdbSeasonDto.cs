using System.Text.Json.Serialization;

namespace OmnisReview.Models.TMDB;

public class TmdbSeasonDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    [JsonPropertyName("season_number")]
    public int SeasonNumber { get; set; }

    [JsonPropertyName("air_date")]
    public DateTime? AirDate { get; set; }

    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }

    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }

    [JsonPropertyName("episodes")]
    public List<TmdbEpisodeDto> Episodes { get; set; } = new();
}

public class TmdbEpisodeDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    [JsonPropertyName("episode_number")]
    public int EpisodeNumber { get; set; }

    [JsonPropertyName("season_number")]
    public int SeasonNumber { get; set; }

    [JsonPropertyName("air_date")]
    public DateTime? AirDate { get; set; }

    [JsonPropertyName("still_path")]
    public string? StillPath { get; set; }

    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }

    [JsonPropertyName("vote_count")]
    public int VoteCount { get; set; }

    [JsonPropertyName("runtime")]
    public int Runtime { get; set; }

    [JsonPropertyName("crew")]
    public List<object> Crew { get; set; } = new();

    [JsonPropertyName("guest_stars")]
    public List<TmdbCastDto> GuestStars { get; set; } = new();
}
