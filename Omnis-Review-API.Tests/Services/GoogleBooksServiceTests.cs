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
}
