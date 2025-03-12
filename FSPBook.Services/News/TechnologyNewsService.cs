using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace FSPBook.Services.News
{
    public class TechnologyNewsService : INewsService
    {
        private readonly ICacheService _cacheService;
        private readonly INewsApiClient _newsApiClient;
        private readonly ILogger<TechnologyNewsService> _logger;
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
        private readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> _circuitBreakerPolicy;

        public TechnologyNewsService(ICacheService cacheService, INewsApiClient newsApiClient, ILogger<TechnologyNewsService> logger)
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
                .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1));
        }

        /// <summary>
        /// Get the top headlines for technology news
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<IEnumerable<NewsArticle>> GetTopHeadlinesAsync(int limit)
        {
            var cacheKey = $"TechHeadlines_{limit}";

            return await _cacheService.GetOrAddAsync(cacheKey, async () =>
            {
                HttpResponseMessage? response = null;
                try
                {
                    response = await _retryPolicy.ExecuteAsync(() =>
                                    _circuitBreakerPolicy.ExecuteAsync(() => _newsApiClient.GetTopHeadlinesAsync(limit)));

                }
                catch (BrokenCircuitException ex)
                {
                    _logger.LogError(ex, "Circuit breaker is open. Unable to connect to the News Api.");
                }

                if (response == null || !response.IsSuccessStatusCode)
                {
                    return new List<NewsArticle>();
                }

                var content = await response?.Content?.ReadAsStringAsync();
                var newsResponse = JsonSerializer.Deserialize<NewsResponse>(content);
                return newsResponse?.Data ?? new List<NewsArticle>();
            }, TimeSpan.FromMinutes(10));
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
