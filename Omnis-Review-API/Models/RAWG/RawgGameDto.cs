using System.Text.Json.Serialization;

namespace OmnisReview.Models.RAWG;

public class RawgGameDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("released")]
    public string? Released { get; set; }

    [JsonPropertyName("tba")]
    public bool Tba { get; set; }

    [JsonPropertyName("background_image")]
    public string? BackgroundImage { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("rating")]
    public double Rating { get; set; }

    [JsonPropertyName("rating_top")]
    public int RatingTop { get; set; }

    [JsonPropertyName("ratings")]
    public List<RawgRatingDto> Ratings { get; set; } = new();

    [JsonPropertyName("reactions_count")]
    public int ReactionsCount { get; set; }

    [JsonPropertyName("added")]
    public int Added { get; set; }

    [JsonPropertyName("updated")]
    public string? Updated { get; set; }

    [JsonPropertyName("genres")]
    public List<RawgGenreDto> Genres { get; set; } = new();

    [JsonPropertyName("platforms")]
    public List<RawgPlatformDto> Platforms { get; set; } = new();

    [JsonPropertyName("playtime")]
    public int Playtime { get; set; }

    [JsonPropertyName("tags")]
    public List<RawgTagDto> Tags { get; set; } = new();

    [JsonPropertyName("website")]
    public string? Website { get; set; }

    [JsonPropertyName("metacritic")]
    public int? MetacriticScore { get; set; }
}
