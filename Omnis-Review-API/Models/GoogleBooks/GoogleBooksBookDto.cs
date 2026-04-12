namespace OmnisReview.Models.GoogleBooks;

public class GoogleBooksBookDto
{
    public string? Id { get; set; }
    public GoogleBooksVolumeInfoDto? VolumeInfo { get; set; }
    public string? ETag { get; set; }
    public string? SelfLink { get; set; }
}
