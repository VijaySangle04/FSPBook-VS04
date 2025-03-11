using Microsoft.Extensions.Caching.Memory;

namespace FSPBook.Services.News
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<IEnumerable<NewsArticle>> GetOrAddAsync(string cacheKey, 
            Func<Task<IEnumerable<NewsArticle>>> fetchFunction, TimeSpan expiration)
        {
            if (_cache.TryGetValue(cacheKey, 
                out IEnumerable<NewsArticle> cachedArticles))
            {
                return cachedArticles;
            }

            var articles = await fetchFunction();
            _cache.Set(cacheKey, articles, 
                new MemoryCacheEntryOptions().SetSlidingExpiration(expiration));
            return articles;
        }
    }
}
