using System.Text.Json.Serialization;

namespace OmnisReview.Models.RAWG;

public class RawgGameDetailDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("released")]
    public string? Released { get; set; }

    [JsonPropertyName("tba")]
    public bool Tba { get; set; }

    [JsonPropertyName("background_image")]
    public string? BackgroundImage { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("rating")]
    public double Rating { get; set; }

    [JsonPropertyName("rating_top")]
    public int RatingTop { get; set; }

    [JsonPropertyName("ratings")]
    public List<RawgRatingDto> Ratings { get; set; } = new();

    [JsonPropertyName("reactions_count")]
    public int ReactionsCount { get; set; }

    [JsonPropertyName("added")]
    public int Added { get; set; }

    [JsonPropertyName("updated")]
    public string? Updated { get; set; }

    [JsonPropertyName("genres")]
    public List<RawgGenreDto> Genres { get; set; } = new();

    [JsonPropertyName("name_original")]
    public string? NameOriginal { get; set; }

    [JsonPropertyName("platforms")]
    public List<RawgGamePlatformDto> Platforms { get; set; } = new();

    [JsonPropertyName("publishers")]
    public List<RawgPublisherDto> Publishers { get; set; } = new();

    [JsonPropertyName("developers")]
    public List<RawgDeveloperDto> Developers { get; set; } = new();

    [JsonPropertyName("playtime")]
    public int Playtime { get; set; }

    [JsonPropertyName("screenshots")]
    public List<RawgScreenshotDto> Screenshots { get; set; } = new();

    [JsonPropertyName("stores")]
    public List<RawgStoreDto> Stores { get; set; } = new();

    [JsonPropertyName("tags")]
    public List<RawgTagDto> Tags { get; set; } = new();

    [JsonPropertyName("website")]
    public string? Website { get; set; }

    [JsonPropertyName("metacritic")]
    public int? MetacriticScore { get; set; }

    [JsonPropertyName("background_image_additional")]
    public string? BackgroundImageAdditional { get; set; }

    [JsonPropertyName("metacritic_url")]
    public string? MetacriticUrl { get; set; }

    [JsonPropertyName("metacritic_platforms")]
    public List<RawgMetacriticPlatformDto> MetacriticPlatforms { get; set; } = new();

    [JsonPropertyName("reddit_url")]
    public string? RedditUrl { get; set; }

    [JsonPropertyName("reddit_name")]
    public string? RedditName { get; set; }

    [JsonPropertyName("reddit_description")]
    public string? RedditDescription { get; set; }

    [JsonPropertyName("reddit_logo")]
    public string? RedditLogo { get; set; }

    [JsonPropertyName("reddit_count")]
    public int RedditCount { get; set; }

     [JsonPropertyName("twitch_count")]
    public int TwitchCount { get; set; }

    [JsonPropertyName("youtube_count")]
    public int YoutubeCount { get; set; }

    [JsonPropertyName("reviews_text_count")]
    public int ReviewsTextCount { get; set; }

    [JsonPropertyName("ratings_count")]
    public int RatingsCount { get; set; }

    [JsonPropertyName("suggestions_count")]
    public int SuggestionsCount { get; set; }

    [JsonPropertyName("alternative_names")]
    public List<string> AlternativeNames { get; set; } = new();

    [JsonPropertyName("esrb_rating")]
    public RawgEsrbRatingDto? EsrbRating { get; set; }

    [JsonPropertyName("screenshots_count")]
    public int ScreenshotsCount { get; set; }

    [JsonPropertyName("movies_count")]
    public int MoviesCount { get; set; }

    [JsonPropertyName("creators_count")]
    public int CreatorsCount { get; set; }

    [JsonPropertyName("achievements_count")]
    public int AchievementsCount { get; set; }

    [JsonPropertyName("parent_achievements_count")]
    public int ParentAchievementsCount { get; set; }

    [JsonPropertyName("parents_count")]
    public int ParentsCount { get; set; }

    [JsonPropertyName("additions_count")]
    public int AdditionsCount { get; set; }

    [JsonPropertyName("game_series_count")]
    public int GameSeriesCount { get; set; }
}
