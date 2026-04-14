using System.Text.Json.Serialization;

namespace OmnisReview.Models.RAWG;

public class RawgEsrbRatingDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
