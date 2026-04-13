namespace OmnisReview.Models.RAWG;

public class RawgGameDto
{
    public int Id { get; set; }
    public string? Slug { get; set; }
    public string? Name { get; set; }
    public string? Released { get; set; }
    public bool Tba { get; set; }
    public string? BackgroundImage { get; set; }
    public string? Description { get; set; }
    public double Rating { get; set; }
    public int RatingTop { get; set; }
    public List<RawgRatingDto> Ratings { get; set; } = new();
    public int ReactionsCount { get; set; }
    public int Added { get; set; }
    public string? Updated { get; set; }
    public List<RawgGenreDto> Genres { get; set; } = new();
    public List<RawgPlatformDto> Platforms { get; set; } = new();
    public List<RawgPublisherDto> Publishers { get; set; } = new();
    public List<RawgDeveloperDto> Developers { get; set; } = new();
    public int Playtime { get; set; }
    public List<RawgScreenshotDto> Screenshots { get; set; } = new();
    public List<RawgStoreDto> Stores { get; set; } = new();
    public List<RawgTagDto> Tags { get; set; } = new();
    public string? Website { get; set; }
    public int MetacriticScore { get; set; }
}
