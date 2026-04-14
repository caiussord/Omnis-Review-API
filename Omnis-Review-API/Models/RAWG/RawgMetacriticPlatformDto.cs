using System.Text.Json.Serialization;

namespace OmnisReview.Models.RAWG;

public class RawgMetacriticPlatformDto
{
    [JsonPropertyName("metascore")]
    public int? Metascore { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }
}
