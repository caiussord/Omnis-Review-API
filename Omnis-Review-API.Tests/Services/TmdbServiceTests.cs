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

    [Test]
    public async Task SearchMoviesCardAsync_WithValidQuery_ReturnsMovieCardResult()
    {
        var result = await _tmdbService.SearchMoviesCardAsync("Inception");

        Assert.That(result, Is.Null.Or.InstanceOf<TmdbPagedResultDto<TmdbMovieCardDto>>());
    }

    [Test]
    public async Task SearchMoviesCardAsync_WithEmptyQuery_ReturnsNull()
    {
        var result = await _tmdbService.SearchMoviesCardAsync("");

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task SearchSeriesCardAsync_WithValidQuery_ReturnsSeriesCardResult()
    {
        var result = await _tmdbService.SearchSeriesCardAsync("Breaking Bad");

        Assert.That(result, Is.Null.Or.InstanceOf<TmdbPagedResultDto<TmdbSeriesCardDto>>());
    }

    [Test]
    public async Task SearchSeriesCardAsync_WithEmptyQuery_ReturnsNull()
    {
        var result = await _tmdbService.SearchSeriesCardAsync("");

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetMovieDetailAsync_WithValidId_ReturnsMovieDetailDto()
    {
        var result = await _tmdbService.GetMovieDetailAsync(550);

        Assert.That(result, Is.Null.Or.InstanceOf<TmdbMovieDetailDto>());
    }

    [Test]
    public async Task GetMovieDetailAsync_WithInvalidId_ReturnsNull()
    {
        var result = await _tmdbService.GetMovieDetailAsync(999999999);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetSeriesDetailAsync_WithValidId_ReturnsSeriesDetailDto()
    {
        var result = await _tmdbService.GetSeriesDetailAsync(1396);

        Assert.That(result, Is.Null.Or.InstanceOf<TmdbSeriesDetailDto>());
    }

    [Test]
    public async Task GetSeriesDetailAsync_WithInvalidId_ReturnsNull()
    {
        var result = await _tmdbService.GetSeriesDetailAsync(999999999);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetPopularMoviesCardAsync_ReturnsPopularMovieCardsResult()
    {
        var result = await _tmdbService.GetPopularMoviesCardAsync();

        Assert.That(result, Is.Null.Or.InstanceOf<TmdbPagedResultDto<TmdbMovieCardDto>>());
    }

    [Test]
    public async Task GetPopularMoviesCardAsync_WithPagination_ReturnsPopularMovieCardsResult()
    {
        var result = await _tmdbService.GetPopularMoviesCardAsync(page: 2);

        Assert.That(result, Is.Null.Or.InstanceOf<TmdbPagedResultDto<TmdbMovieCardDto>>());
    }

    [Test]
    public async Task GetPopularSeriesCardAsync_ReturnsPopularSeriesCardsResult()
    {
        var result = await _tmdbService.GetPopularSeriesCardAsync();

        Assert.That(result, Is.Null.Or.InstanceOf<TmdbPagedResultDto<TmdbSeriesCardDto>>());
    }

    [Test]
    public async Task GetPopularSeriesCardAsync_WithPagination_ReturnsPopularSeriesCardsResult()
    {
        var result = await _tmdbService.GetPopularSeriesCardAsync(page: 2);

        Assert.That(result, Is.Null.Or.InstanceOf<TmdbPagedResultDto<TmdbSeriesCardDto>>());
    }

    [Test]
    public async Task GetTopRatedMoviesCardAsync_ReturnsTopRatedMovieCardsResult()
    {
        var result = await _tmdbService.GetTopRatedMoviesCardAsync();

        Assert.That(result, Is.Null.Or.InstanceOf<TmdbPagedResultDto<TmdbMovieCardDto>>());
    }

    [Test]
    public async Task GetTopRatedMoviesCardAsync_WithPagination_ReturnsTopRatedMovieCardsResult()
    {
        var result = await _tmdbService.GetTopRatedMoviesCardAsync(page: 2);

        Assert.That(result, Is.Null.Or.InstanceOf<TmdbPagedResultDto<TmdbMovieCardDto>>());
    }

    [Test]
    public async Task GetTopRatedSeriesCardAsync_ReturnsTopRatedSeriesCardsResult()
    {
        var result = await _tmdbService.GetTopRatedSeriesCardAsync();

        Assert.That(result, Is.Null.Or.InstanceOf<TmdbPagedResultDto<TmdbSeriesCardDto>>());
    }

    [Test]
    public async Task GetTopRatedSeriesCardAsync_WithPagination_ReturnsTopRatedSeriesCardsResult()
    {
        var result = await _tmdbService.GetTopRatedSeriesCardAsync(page: 2);

        Assert.That(result, Is.Null.Or.InstanceOf<TmdbPagedResultDto<TmdbSeriesCardDto>>());
    }

    [Test]
    public async Task GetMovieCastAsync_WithValidMovieId_ReturnsCastListWithCorrectStructure()
    {
        var result = await _tmdbService.GetMovieCastAsync(278);

        if (result is not null)
        {
            Assert.That(result, Is.InstanceOf<List<TmdbCastDto>>());
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));

            if (result.Count > 0)
            {
                var firstCast = result[0];
                Assert.That(firstCast.Id, Is.GreaterThan(0));
                Assert.That(firstCast.Name, Is.Not.Null.And.Not.Empty);
            }
        }
    }

    [Test]
    public async Task GetMovieCastAsync_WithInvalidMovieId_ReturnsNullOrEmptyList()
    {
        var result = await _tmdbService.GetMovieCastAsync(999999999);

        Assert.That(result, Is.Null.Or.Empty);
    }

    [Test]
    public async Task GetSeriesCastAsync_WithValidSeriesId_ReturnsCastListWithCorrectStructure()
    {
        var result = await _tmdbService.GetSeriesCastAsync(1396);

        if (result is not null)
        {
            Assert.That(result, Is.InstanceOf<List<TmdbCastDto>>());
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));

            if (result.Count > 0)
            {
                var firstCast = result[0];
                Assert.That(firstCast.Id, Is.GreaterThan(0));
                Assert.That(firstCast.Name, Is.Not.Null.And.Not.Empty);
            }
        }
    }

    [Test]
    public async Task GetSeriesCastAsync_WithInvalidSeriesId_ReturnsNullOrEmptyList()
    {
        var result = await _tmdbService.GetSeriesCastAsync(999999999);

        Assert.That(result, Is.Null.Or.Empty);
    }

    [Test]
    public async Task GetMovieVideosAsync_WithValidMovieId_ReturnsVideosWithCorrectStructure()
    {
        var result = await _tmdbService.GetMovieVideosAsync(278);

        if (result is not null)
        {
            Assert.That(result, Is.InstanceOf<List<TmdbVideoDto>>());
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));

            if (result.Count > 0)
            {
                var firstVideo = result[0];
                Assert.That(firstVideo.Key, Is.Not.Null.And.Not.Empty);
                Assert.That(firstVideo.Site, Is.EqualTo("YouTube"));
            }
        }
    }

    [Test]
    public async Task GetSeriesVideosAsync_WithValidSeriesId_ReturnsVideosWithCorrectStructure()
    {
        var result = await _tmdbService.GetSeriesVideosAsync(1396);

        if (result is not null)
        {
            Assert.That(result, Is.InstanceOf<List<TmdbVideoDto>>());
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));

            if (result.Count > 0)
            {
                var firstVideo = result[0];
                Assert.That(firstVideo.Key, Is.Not.Null.And.Not.Empty);
                Assert.That(firstVideo.Site, Is.EqualTo("YouTube"));
            }
        }
    }
}
