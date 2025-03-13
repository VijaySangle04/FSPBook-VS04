using Microsoft.Extensions.Caching.Memory;

namespace FSPBook.Services.Caching
{
    public class CacheService<T> : ICacheService<T>
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<IEnumerable<T>> GetOrAddAsync(string cacheKey,
            Func<Task<IEnumerable<T>>> fetchFunction, TimeSpan expiration)
        {
            if (_cache.TryGetValue(cacheKey,
                out IEnumerable<T> cachedValues))
            {
                return cachedValues;
            }

            var articles = await fetchFunction();
            _cache.Set(cacheKey, articles,
                new MemoryCacheEntryOptions().SetSlidingExpiration(expiration));
            return articles;
        }
    }
}
