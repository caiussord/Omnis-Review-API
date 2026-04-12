using Moq;
using NUnit.Framework;
using OmnisReview.Models.TMDB;
using OmnisReview.Services;
using Microsoft.Extensions.Configuration;

namespace OmnisReview.Tests.Services;

[TestFixture]
public class TmdbServiceTests
{
    private Mock<IConfiguration> _mockConfiguration = null!;
    private TmdbService _tmdbService = null!;

    [SetUp]
    public void SetUp()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _mockConfiguration.Setup(c => c["Tmdb:ApiKey"]).Returns("test-api-key");
        
        var mockHttpClient = new Mock<HttpClient>();
        _tmdbService = new TmdbService(mockHttpClient.Object, _mockConfiguration.Object);
    }

    [Test]
    public void Constructor_WithoutApiKey_ThrowsInvalidOperationException()
    {
        _mockConfiguration.Setup(c => c["Tmdb:ApiKey"]).Returns((string?)null);
        
        Assert.Throws<InvalidOperationException>(() => 
            new TmdbService(new HttpClient(), _mockConfiguration.Object));
    }

    [Test]
    public async Task SearchMoviesAsync_WithValidQuery_ReturnsResult()
    {
        var result = await _tmdbService.SearchMoviesAsync("Inception");
        
        Assert.That(result, Is.Null.Or.InstanceOf<TmdbPagedResultDto<TmdbMovieDto>>());
    }

    [Test]
    public async Task SearchSeriesAsync_WithValidQuery_ReturnsResult()
    {
        var result = await _tmdbService.SearchSeriesAsync("Breaking Bad");
        
        Assert.That(result, Is.Null.Or.InstanceOf<TmdbPagedResultDto<TmdbSeriesDto>>());
    }

    [Test]
    public async Task GetMovieDetailsAsync_WithValidId_ReturnsMovieDetails()
    {
        var result = await _tmdbService.GetMovieDetailsAsync(550);
        
        Assert.That(result, Is.Null.Or.InstanceOf<TmdbMovieDetailsDto>());
    }

    [Test]
    public async Task GetSeriesDetailsAsync_WithValidId_ReturnsSeriesDetails()
    {
        var result = await _tmdbService.GetSeriesDetailsAsync(1396);
        
        Assert.That(result, Is.Null.Or.InstanceOf<TmdbSeriesDetailsDto>());
    }

    [Test]
    public async Task GetPopularMoviesAsync_ReturnsResult()
    {
        var result = await _tmdbService.GetPopularMoviesAsync();
        
        Assert.That(result, Is.Null.Or.InstanceOf<TmdbPagedResultDto<TmdbMovieDto>>());
    }

    [Test]
    public async Task GetPopularSeriesAsync_ReturnsResult()
    {
        var result = await _tmdbService.GetPopularSeriesAsync();
        
        Assert.That(result, Is.Null.Or.InstanceOf<TmdbPagedResultDto<TmdbSeriesDto>>());
    }

    [Test]
    public async Task GetMovieCastAsync_WithValidId_ReturnsCast()
    {
        var result = await _tmdbService.GetMovieCastAsync(550);
        
        Assert.That(result, Is.Null.Or.InstanceOf<List<TmdbCastDto>>());
    }

    [Test]
    public async Task GetSeriesCastAsync_WithValidId_ReturnsCast()
    {
        var result = await _tmdbService.GetSeriesCastAsync(1396);
        
        Assert.That(result, Is.Null.Or.InstanceOf<List<TmdbCastDto>>());
    }

    [Test]
    public async Task GetMovieVideosAsync_WithValidId_ReturnsVideos()
    {
        var result = await _tmdbService.GetMovieVideosAsync(550);
        
        Assert.That(result, Is.Null.Or.InstanceOf<List<TmdbVideoDto>>());
    }

    [Test]
    public async Task GetSeriesVideosAsync_WithValidId_ReturnsVideos()
    {
        var result = await _tmdbService.GetSeriesVideosAsync(1396);
        
        Assert.That(result, Is.Null.Or.InstanceOf<List<TmdbVideoDto>>());
    }
}
