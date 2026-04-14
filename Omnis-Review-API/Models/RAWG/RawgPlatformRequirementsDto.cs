using System.Text.Json.Serialization;

namespace OmnisReview.Models.RAWG;

public class RawgPlatformRequirementsDto
{
    [JsonPropertyName("minimum")]
    public string? Minimum { get; set; }

    [JsonPropertyName("recommended")]
    public string? Recommended { get; set; }
}
