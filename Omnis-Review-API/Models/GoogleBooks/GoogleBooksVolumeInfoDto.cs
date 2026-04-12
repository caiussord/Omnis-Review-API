namespace OmnisReview.Models.GoogleBooks;

public class GoogleBooksVolumeInfoDto
{
    public string? Title { get; set; }
    public List<string> Authors { get; set; } = new();
    public string? Publisher { get; set; }
    public string? PublishedDate { get; set; }
    public string? Description { get; set; }
    public int PageCount { get; set; }
    public List<string> Categories { get; set; } = new();
    public double AverageRating { get; set; }
    public int RatingsCount { get; set; }
    public string? Language { get; set; }
    public string? PreviewLink { get; set; }
    public string? InfoLink { get; set; }
    public string? CanonicalVolumeLink { get; set; }
    public GoogleBooksImageLinksDto? ImageLinks { get; set; }
    public string? ISBN10 { get; set; }
    public string? ISBN13 { get; set; }
}
