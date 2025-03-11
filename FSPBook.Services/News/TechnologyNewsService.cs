using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FSPBook.Services.News
{
    public class TechnologyNewsService : INewsService
    {
        private readonly ICacheService _cacheService;
        private readonly INewsApiClient _newsApiClient;
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
        private readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> _circuitBreakerPolicy;

        public TechnologyNewsService(ICacheService cacheService, INewsApiClient newsApiClient)
        {
            _cacheService = cacheService;
            _newsApiClient = newsApiClient;
            _retryPolicy = Policy.Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            _circuitBreakerPolicy = Policy.Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1));
        }

        public async Task<IEnumerable<NewsArticle>> GetTopHeadlinesAsync(int limit, bool clearCache = false)
        {
            var cacheKey = $"TechHeadlines_{limit}";

            return await _cacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var response = await _retryPolicy.ExecuteAsync(() =>
                    _circuitBreakerPolicy.ExecuteAsync(() => _newsApiClient.GetTopHeadlinesAsync(limit)));

                if (!response.IsSuccessStatusCode)
                {
                    return new List<NewsArticle>();
                }

                var content = await response.Content.ReadAsStringAsync();
                var newsResponse = JsonSerializer.Deserialize<NewsResponse>(content);
                return newsResponse?.Data ?? new List<NewsArticle>();
            }, TimeSpan.FromMinutes(30));
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
