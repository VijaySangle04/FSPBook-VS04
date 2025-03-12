using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;

namespace FSPBook.Services.News.Tests
{
    [TestClass]
    public class TechnologyNewsServiceTests
    {
        private Mock<ICacheService> _mockCacheService;
        private Mock<INewsApiClient> _mockNewsApiClient;
        private Mock<ILogger<TechnologyNewsService>> _mockLogger;
        private TechnologyNewsService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockCacheService = new Mock<ICacheService>();
            _mockNewsApiClient = new Mock<INewsApiClient>();
            _mockLogger = new Mock<ILogger<TechnologyNewsService>>();
            _service = new TechnologyNewsService(_mockCacheService.Object, _mockNewsApiClient.Object, _mockLogger.Object);
        }

        [TestMethod]
        public async Task GetTopHeadlinesAsync_ReturnsCachedArticles_WhenCacheIsAvailable()
        {
            // Arrange
            var cachedArticles = new List<NewsArticle> { new NewsArticle { Title = "Cached News", Url = "http://cachednews.com" } };
            _mockCacheService.Setup(c => c.GetOrAddAsync(It.IsAny<string>(), It.IsAny<Func<Task<IEnumerable<NewsArticle>>>>(), It.IsAny<TimeSpan>()))
                             .ReturnsAsync(cachedArticles);

            // Act
            var result = await _service.GetTopHeadlinesAsync(5);

            // Assert
            Assert.AreEqual(cachedArticles, result);
        }

        [TestMethod]
        public async Task GetTopHeadlinesAsync_ReturnsFetchedArticles_WhenCacheIsNotAvailable()
        {
            // Arrange
            var fetchedArticles = new List<NewsArticle> { new NewsArticle { Title = "Fetched News", Url = "http://fetchednews.com" } };
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(new NewsResponse { Data = fetchedArticles }))
            };
            _mockNewsApiClient.Setup(n => n.GetTopHeadlinesAsync(It.IsAny<int>())).ReturnsAsync(response);
            _mockCacheService.Setup(c => c.GetOrAddAsync(It.IsAny<string>(), It.IsAny<Func<Task<IEnumerable<NewsArticle>>>>(), It.IsAny<TimeSpan>()))
                             .Returns((string key, Func<Task<IEnumerable<NewsArticle>>> fetchFunction, TimeSpan expiration) => fetchFunction());

            // Act
            var result = await _service.GetTopHeadlinesAsync(5);

            // Assert
            Assert.AreEqual(fetchedArticles.FirstOrDefault().Title, result.FirstOrDefault().Title);
        }

        [TestMethod]
        public async Task GetTopHeadlinesAsync_ReturnsEmptyList_WhenApiResponseIsNotSuccessful()
        {
            // Arrange
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            _mockNewsApiClient.Setup(n => n.GetTopHeadlinesAsync(It.IsAny<int>())).ReturnsAsync(response);
            _mockCacheService.Setup(c => c.GetOrAddAsync(It.IsAny<string>(), It.IsAny<Func<Task<IEnumerable<NewsArticle>>>>(), It.IsAny<TimeSpan>()))
                             .Returns((string key, Func<Task<IEnumerable<NewsArticle>>> fetchFunction, TimeSpan expiration) => fetchFunction());

            // Act
            var result = await _service.GetTopHeadlinesAsync(5);

            // Assert
            Assert.AreEqual(0, result.Count());
        }
    }
}