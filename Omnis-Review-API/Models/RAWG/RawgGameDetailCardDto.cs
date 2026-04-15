namespace OmnisReview.Models.RAWG;

public class RawgGameDetailCardDto
{
    public int Id { get; set; }
    public string? BackgroundImage { get; set; }
    public string? Name { get; set; }
    public double Rating { get; set; }
    public List<RawgGenreDto> Genres { get; set; } = new();
    public MediaType Type => MediaType.Game;
    public string? Description { get; set; }
    public string? Released { get; set; }
    public List<RawgDeveloperDto> Developers { get; set; } = new();
    public List<RawgPublisherDto> Publishers { get; set; } = new();
}
