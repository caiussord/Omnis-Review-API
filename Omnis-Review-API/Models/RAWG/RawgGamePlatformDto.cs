using System.Text.Json.Serialization;

namespace OmnisReview.Models.RAWG;

public class RawgGamePlatformDto
{
    [JsonPropertyName("platform")]
    public RawgPlatformDto? Platform { get; set; }

    [JsonPropertyName("released_at")]
    public string? ReleasedAt { get; set; }

    [JsonPropertyName("requirements")]
    public RawgPlatformRequirementsDto? Requirements { get; set; }
}
