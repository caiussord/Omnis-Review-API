namespace OmnisReview.Models.RAWG;

public class RawgPagedResultDto
{
    public int Count { get; set; }
    public string? Next { get; set; }
    public string? Previous { get; set; }
    public List<RawgGameDto> Results { get; set; } = new();
}
