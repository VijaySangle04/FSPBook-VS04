using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using FSPBook.Services.Caching;

namespace FSPBook.Services.News
{
    public class TechnologyNewsService : INewsService
    {
        private readonly ICacheService<NewsArticle> _cacheService;
        private readonly INewsApiClient _newsApiClient;
        private readonly ILogger<TechnologyNewsService> _logger;
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
        private readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> _circuitBreakerPolicy;

        public TechnologyNewsService(ICacheService<NewsArticle> cacheService, INewsApiClient newsApiClient, ILogger<TechnologyNewsService> logger)
        {
            _cacheService = cacheService;
            _newsApiClient = newsApiClient;
            _logger = logger;
            _retryPolicy = Policy.Handle<HttpRequestException>()
                                 .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                                 .WaitAndRetryAsync(3,
                                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            _circuitBreakerPolicy = Policy.Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .CircuitBreakerAsync(2, TimeSpan.FromMinutes(5));
        }

        /// <summary>
        /// Get the top headlines for technology news
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<IEnumerable<NewsArticle>> GetTopHeadlinesAsync(int limit)
        {
            var cacheKey = $"TechHeadlines_{limit}";
            HttpResponseMessage? response = null;
            try
            {
                return await _cacheService.GetOrAddAsync(cacheKey, async () =>
                {
                    response = await _retryPolicy.ExecuteAsync(() =>
                                    _circuitBreakerPolicy.ExecuteAsync(() => _newsApiClient.GetTopHeadlinesAsync(limit)));
                    var content = await response?.Content?.ReadAsStringAsync();
                    var newsResponse = JsonSerializer.Deserialize<NewsResponse>(content);
                    return newsResponse?.Data;
                }, TimeSpan.FromMinutes(5));
            }
            catch (BrokenCircuitException ex)
            {
                _logger.LogError(ex, "Circuit breaker is open. Unable to connect to the News Api.");
            }
            return new List<NewsArticle>();
        }
    }

    public class NewsResponse
    {
        [JsonPropertyName("data")]
        public List<NewsArticle> Data { get; set; } = new List<NewsArticle>();
    }

    public class NewsArticle
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }
}
