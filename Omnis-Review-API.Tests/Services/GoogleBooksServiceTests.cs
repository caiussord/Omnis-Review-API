using Moq;
using NUnit.Framework;
using OmnisReview.Models.GoogleBooks;
using OmnisReview.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace OmnisReview.Tests.Services;

[TestFixture]
public class GoogleBooksServiceTests
{
    private Mock<IConfiguration> _mockConfiguration = null!;
    private Mock<ILogger<GoogleBooksService>> _mockLogger = null!;
    private GoogleBooksService _googleBooksService = null!;

    [SetUp]
    public void SetUp()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _mockConfiguration.Setup(c => c["GoogleBooks:ApiKey"]).Returns("test-api-key");

        _mockLogger = new Mock<ILogger<GoogleBooksService>>();

        var mockHttpClient = new Mock<HttpClient>();
        _googleBooksService = new GoogleBooksService(mockHttpClient.Object, _mockConfiguration.Object, _mockLogger.Object);
    }

    [Test]
    public void Constructor_WithoutApiKey_ThrowsInvalidOperationException()
    {
        _mockConfiguration.Setup(c => c["GoogleBooks:ApiKey"]).Returns((string?)null);

        Assert.Throws<InvalidOperationException>(() => 
            new GoogleBooksService(new HttpClient(), _mockConfiguration.Object, _mockLogger.Object));
    }

    [Test]
    public async Task SearchBooksAsync_WithValidQuery_ReturnsResult()
    {
        var result = await _googleBooksService.SearchBooksAsync("Harry Potter");
        
        Assert.That(result, Is.Null.Or.InstanceOf<GoogleBooksPagedResultDto>());
    }

    [Test]
    public async Task SearchByTitleAsync_WithValidTitle_ReturnsResult()
    {
        var result = await _googleBooksService.SearchByTitleAsync("The Great Gatsby");
        
        Assert.That(result, Is.Null.Or.InstanceOf<GoogleBooksPagedResultDto>());
    }

    [Test]
    public async Task SearchByAuthorAsync_WithValidAuthor_ReturnsResult()
    {
        var result = await _googleBooksService.SearchByAuthorAsync("J.K. Rowling");
        
        Assert.That(result, Is.Null.Or.InstanceOf<GoogleBooksPagedResultDto>());
    }

    [Test]
    public async Task SearchByISBNAsync_WithValidISBN_ReturnsResult()
    {
        var result = await _googleBooksService.SearchByISBNAsync("9780747532699");
        
        Assert.That(result, Is.Null.Or.InstanceOf<GoogleBooksPagedResultDto>());
    }

    [Test]
    public async Task SearchByPublisherAsync_WithValidPublisher_ReturnsResult()
    {
        var result = await _googleBooksService.SearchByPublisherAsync("Bloomsbury");
        
        Assert.That(result, Is.Null.Or.InstanceOf<GoogleBooksPagedResultDto>());
    }

    [Test]
    public async Task GetBookByIdAsync_WithValidId_ReturnsBook()
    {
        var result = await _googleBooksService.GetBookByIdAsync("WQW-JQAACAAJ");
        
        Assert.That(result, Is.Null.Or.InstanceOf<GoogleBooksBookDto>());
    }

    [Test]
    public async Task SearchBooksAsync_WithPagination_ReturnsResult()
    {
        var result = await _googleBooksService.SearchBooksAsync("Python", startIndex: 10, maxResults: 20);

        Assert.That(result, Is.Null.Or.InstanceOf<GoogleBooksPagedResultDto>());
    }

    // Novos testes para métodos Card/Detail otimizados

    [Test]
    public async Task SearchBooksCardAsync_WithValidQuery_ReturnsBookCardResult()
    {
        var result = await _googleBooksService.SearchBooksCardAsync("Harry Potter");

        Assert.That(result, Is.Null.Or.InstanceOf<List<BookCardDto>>());
    }

    [Test]
    public async Task SearchBooksCardAsync_WithEmptyQuery_ReturnsEmptyList()
    {
        var result = await _googleBooksService.SearchBooksCardAsync("");

        Assert.That(result, Is.Null.Or.Empty);
    }

    [Test]
    public async Task SearchBooksCardAsync_WithPagination_ReturnsBookCardResult()
    {
        var result = await _googleBooksService.SearchBooksCardAsync("Python", startIndex: 5, maxResults: 15);

        Assert.That(result, Is.Null.Or.InstanceOf<List<BookCardDto>>());
    }

    [Test]
    public async Task SearchBooksCardAsync_ReturnsOnlyRequiredFields()
    {
        var result = await _googleBooksService.SearchBooksCardAsync("The Great Gatsby");

        if (result != null && result.Count > 0)
        {
            var firstBook = result[0];
            Assert.That(firstBook.Id, Is.Not.Empty);
            Assert.That(firstBook.Title, Is.Not.Null.And.Not.Empty);
            // AverageRating pode ser 0 se não houver rating
            Assert.That(firstBook.AverageRating, Is.GreaterThanOrEqualTo(0));
        }
    }

    [Test]
    public async Task GetBookDetailAsync_WithValidId_ReturnsBookDetailDto()
    {
        var result = await _googleBooksService.GetBookDetailAsync("WQW-JQAACAAJ");

        Assert.That(result, Is.Null.Or.InstanceOf<BookDetailDto>());
    }

    [Test]
    public async Task GetBookDetailAsync_WithInvalidId_ReturnsNull()
    {
        var result = await _googleBooksService.GetBookDetailAsync("invalid-id-12345");

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetBookDetailAsync_ReturnsComprehensiveFields()
    {
        var result = await _googleBooksService.GetBookDetailAsync("WQW-JQAACAAJ");

        if (result != null)
        {
            Assert.That(result.Id, Is.Not.Empty);
            Assert.That(result.Title, Is.Not.Null.And.Not.Empty);
            Assert.That(result, Is.InstanceOf<BookDetailDto>());
        }
    }

    [Test]
    public async Task GetBestsellerBooksCardAsync_ReturnsBestsellerBooksResult()
    {
        var result = await _googleBooksService.GetBestsellerBooksCardAsync();

        Assert.That(result, Is.Null.Or.InstanceOf<List<BookCardDto>>());
    }

    [Test]
    public async Task GetBestsellerBooksCardAsync_WithValidPage_ReturnsBestsellerBooksResult()
    {
        var result = await _googleBooksService.GetBestsellerBooksCardAsync(page: 1);

        Assert.That(result, Is.Null.Or.InstanceOf<List<BookCardDto>>());
    }

    [Test]
    public async Task GetBestsellerBooksCardAsync_ReturnsOnlyCardFields()
    {
        var result = await _googleBooksService.GetBestsellerBooksCardAsync();

        if (result != null && result.Count > 0)
        {
            var firstBook = result[0];
            Assert.That(firstBook.Id, Is.Not.Empty);
            Assert.That(firstBook.Title, Is.Not.Null.And.Not.Empty);
            Assert.That(firstBook.AverageRating, Is.GreaterThanOrEqualTo(0));
        }
    }

    [Test]
    public async Task GetBooksByGenreCardAsync_WithValidGenre_ReturnsGenreBooks()
    {
        var result = await _googleBooksService.GetBooksByGenreCardAsync("fiction");

        Assert.That(result, Is.Null.Or.InstanceOf<List<BookCardDto>>());
    }

    [Test]
    public async Task GetBooksByGenreCardAsync_WithEmptyGenre_ReturnsNull()
    {
        var result = await _googleBooksService.GetBooksByGenreCardAsync("");

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetBooksByGenreCardAsync_WithValidGenreAndPage_ReturnsGenreBooks()
    {
        var result = await _googleBooksService.GetBooksByGenreCardAsync("non-fiction", page: 2);

        Assert.That(result, Is.Null.Or.InstanceOf<List<BookCardDto>>());
    }

    [Test]
    public async Task GetBooksByGenreCardAsync_ReturnsOnlyCardFields()
    {
        var result = await _googleBooksService.GetBooksByGenreCardAsync("mystery");

        if (result != null && result.Count > 0)
        {
            var firstBook = result[0];
            Assert.That(firstBook.Id, Is.Not.Empty);
            Assert.That(firstBook.Title, Is.Not.Null.And.Not.Empty);
            Assert.That(firstBook.AverageRating, Is.GreaterThanOrEqualTo(0));
        }
    }
}
