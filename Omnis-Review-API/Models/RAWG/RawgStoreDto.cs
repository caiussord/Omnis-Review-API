using System.Text.Json.Serialization;

namespace OmnisReview.Models.RAWG;

public class RawgStoreDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("domain")]
    public string? Domain { get; set; }
}
