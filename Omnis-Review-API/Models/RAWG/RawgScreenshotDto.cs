using System.Text.Json.Serialization;

namespace OmnisReview.Models.RAWG;

public class RawgScreenshotDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("image")]
    public string? Image { get; set; }
}
