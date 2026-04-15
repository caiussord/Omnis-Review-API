namespace OmnisReview.Models.GoogleBooks;

public class BookDetailDto
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public double AverageRating { get; set; }
    public List<string> Categories { get; set; } = new();
    public string? CoverImage { get; set; }
    public MediaType Type => MediaType.Book;
    public List<string> Authors { get; set; } = new();
    public string? Publisher { get; set; }
    public string? PublishedDate { get; set; }
    public string? Description { get; set; }
    public int PageCount { get; set; }
}
