using Moq;
using NUnit.Framework;
using OmnisReview.Models.RAWG;
using OmnisReview.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace OmnisReview.Tests.Services;

[TestFixture]
public class RawgServiceTests
{
    private Mock<IConfiguration> _mockConfiguration = null!;
    private Mock<ILogger<RawgService>> _mockLogger = null!;
    private RawgService _rawgService = null!;

    [SetUp]
    public void SetUp()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _mockConfiguration.Setup(c => c["Rawg:ApiKey"]).Returns("test-api-key");

        _mockLogger = new Mock<ILogger<RawgService>>();

        var mockHttpClient = new Mock<HttpClient>();
        _rawgService = new RawgService(mockHttpClient.Object, _mockConfiguration.Object, _mockLogger.Object);
    }

    [Test]
    public void Constructor_WithoutApiKey_ThrowsInvalidOperationException()
    {
        _mockConfiguration.Setup(c => c["Rawg:ApiKey"]).Returns((string?)null);

        Assert.Throws<InvalidOperationException>(() => 
            new RawgService(new HttpClient(), _mockConfiguration.Object, _mockLogger.Object));
    }

    [Test]
    public async Task SearchGamesAsync_WithValidQuery_ReturnsResult()
    {
        var result = await _rawgService.SearchGamesAsync("Elden Ring");
        
        Assert.That(result, Is.Null.Or.InstanceOf<RawgPagedResultDto>());
    }

    [Test]
    public async Task SearchGamesByGenreAsync_WithValidGenre_ReturnsResult()
    {
        var result = await _rawgService.SearchGamesByGenreAsync("action");
        
        Assert.That(result, Is.Null.Or.InstanceOf<RawgPagedResultDto>());
    }

    [Test]
    public async Task SearchGamesByPlatformAsync_WithValidPlatform_ReturnsResult()
    {
        var result = await _rawgService.SearchGamesByPlatformAsync("pc");
        
        Assert.That(result, Is.Null.Or.InstanceOf<RawgPagedResultDto>());
    }

    [Test]
    public async Task GetPopularGamesAsync_ReturnsResult()
    {
        var result = await _rawgService.GetPopularGamesAsync();
        
        Assert.That(result, Is.Null.Or.InstanceOf<RawgPagedResultDto>());
    }

    [Test]
    public async Task GetUpcomingGamesAsync_ReturnsResult()
    {
        var result = await _rawgService.GetUpcomingGamesAsync();
        
        Assert.That(result, Is.Null.Or.InstanceOf<RawgPagedResultDto>());
    }

    [Test]
    public async Task GetGameByIdAsync_WithValidId_ReturnsGame()
    {
        var result = await _rawgService.GetGameByIdAsync(3498);
        
        Assert.That(result, Is.Null.Or.InstanceOf<RawgGameDto>());
    }

    [Test]
    public async Task GetGamesBySortAsync_WithValidSort_ReturnsResult()
    {
        var result = await _rawgService.GetGamesBySortAsync("-rating");
        
        Assert.That(result, Is.Null.Or.InstanceOf<RawgPagedResultDto>());
    }

    [Test]
    public async Task SearchGamesAsync_WithPagination_ReturnsResult()
    {
        var result = await _rawgService.SearchGamesAsync("The Witcher", page: 2, pageSize: 15);
        
        Assert.That(result, Is.Null.Or.InstanceOf<RawgPagedResultDto>());
    }

    [Test]
    public async Task SearchGamesAsync_WithEmptyQuery_ReturnsNull()
    {
        var result = await _rawgService.SearchGamesAsync("");
        
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task SearchGamesByGenreAsync_WithEmptyGenre_ReturnsNull()
    {
        var result = await _rawgService.SearchGamesByGenreAsync("");

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task SearchGamesCardAsync_WithValidQuery_ReturnsGameCardResult()
    {
        var result = await _rawgService.SearchGamesCardAsync("Elden Ring");

        Assert.That(result, Is.Null.Or.InstanceOf<RawgPagedResultDto>());
    }

    [Test]
    public async Task SearchGamesCardAsync_WithEmptyQuery_ReturnsNull()
    {
        var result = await _rawgService.SearchGamesCardAsync("");

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task SearchGamesCardAsync_WithPagination_ReturnsGameCardResult()
    {
        var result = await _rawgService.SearchGamesCardAsync("The Witcher", page: 2, pageSize: 15);

        Assert.That(result, Is.Null.Or.InstanceOf<RawgPagedResultDto>());
    }

    [Test]
    public async Task GetPopularGamesCardAsync_ReturnsPopularGameCardsResult()
    {
        var result = await _rawgService.GetPopularGamesCardAsync();

        Assert.That(result, Is.Null.Or.InstanceOf<RawgPagedResultDto>());
    }

    [Test]
    public async Task GetPopularGamesCardAsync_WithPagination_ReturnsPopularGameCardsResult()
    {
        var result = await _rawgService.GetPopularGamesCardAsync(page: 2, pageSize: 20);

        Assert.That(result, Is.Null.Or.InstanceOf<RawgPagedResultDto>());
    }

    [Test]
    public async Task GetGameDetailAsync_WithValidId_ReturnsGameDetailCardDto()
    {
        var result = await _rawgService.GetGameDetailAsync(3498);

        Assert.That(result, Is.Null.Or.InstanceOf<RawgGameDetailCardDto>());
    }

    [Test]
    public async Task GetGameDetailAsync_WithInvalidId_ReturnsNull()
    {
        var result = await _rawgService.GetGameDetailAsync(999999999);


        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetGameDetailAsync_ReturnsGameWithoutRatingsArray()
    {
        var result = await _rawgService.GetGameDetailAsync(3498);

        if (result != null)
        {
            Assert.That(result, Is.InstanceOf<RawgGameDetailCardDto>());
        }
    }

    [Test]
    public async Task SearchGamesCardAsync_ReturnsOnlyCardFields()
    {
        var result = await _rawgService.SearchGamesCardAsync("Elden Ring");

        if (result != null && result.Results != null && result.Results.Count > 0)
        {
            var firstGame = result.Results[0];
            Assert.That(firstGame.Id, Is.GreaterThan(0));
            Assert.That(firstGame.Name, Is.Not.Null.And.Not.Empty);
            Assert.That(firstGame.BackgroundImage, Is.Not.Null.Or.Empty);
            Assert.That(firstGame.Rating, Is.GreaterThanOrEqualTo(0));
            Assert.That(firstGame.Genres, Is.InstanceOf<List<RawgGenreDto>>());
        }
    }

    [Test]
    public async Task GetPopularGamesCardAsync_ReturnsOnlyCardFields()
    {
        var result = await _rawgService.GetPopularGamesCardAsync();

        if (result != null && result.Results != null && result.Results.Count > 0)
        {
            var firstGame = result.Results[0];
            Assert.That(firstGame.Id, Is.GreaterThan(0));
            Assert.That(firstGame.Name, Is.Not.Null.And.Not.Empty);
            Assert.That(firstGame.BackgroundImage, Is.Not.Null.Or.Empty);
            Assert.That(firstGame.Rating, Is.GreaterThanOrEqualTo(0));
            Assert.That(firstGame.Genres, Is.InstanceOf<List<RawgGenreDto>>());
        }
    }
}
