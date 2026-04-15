namespace OmnisReview.Models.GoogleBooks;

public class BookCardDto
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public double AverageRating { get; set; }
    public List<string> Categories { get; set; } = new();
    public string? CoverImage { get; set; }
    public MediaType Type => MediaType.Book;
}
