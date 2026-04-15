namespace OmnisReview.Models.RAWG;

public class RawgGameCardDto
{
    public int Id { get; set; }
    public string? BackgroundImage { get; set; }
    public string? Name { get; set; }
    public double Rating { get; set; }
    public List<RawgGenreDto> Genres { get; set; } = new();
    public MediaType Type => MediaType.Game;
}
