namespace OmnisReview.Models.GoogleBooks;

public class GoogleBooksPagedResultDto
{
    public string? Kind { get; set; }
    public int TotalItems { get; set; }
    public List<GoogleBooksBookDto> Items { get; set; } = new();
}
