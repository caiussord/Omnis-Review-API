using OmnisReview.Models.GoogleBooks;

namespace OmnisReview.Services.Interfaces;

public interface IGoogleBooksService
{
    Task<GoogleBooksPagedResultDto?> SearchBooksAsync(string query, int startIndex = 0, int maxResults = 10);
    Task<GoogleBooksPagedResultDto?> SearchByTitleAsync(string title, int startIndex = 0, int maxResults = 10);
    Task<GoogleBooksPagedResultDto?> SearchByAuthorAsync(string author, int startIndex = 0, int maxResults = 10);
    Task<GoogleBooksPagedResultDto?> SearchByISBNAsync(string isbn);
    Task<GoogleBooksPagedResultDto?> SearchByPublisherAsync(string publisher, int startIndex = 0, int maxResults = 10);
    Task<GoogleBooksBookDto?> GetBookByIdAsync(string bookId);

    Task<List<BookCardDto>?> SearchBooksCardAsync(string query, int startIndex = 0, int maxResults = 10);
    Task<BookDetailDto?> GetBookDetailAsync(string bookId);

    Task<List<BookCardDto>?> GetBestsellerBooksCardAsync(int page = 0);
    Task<List<BookCardDto>?> GetBooksByGenreCardAsync(string genre, int page = 0);
}
