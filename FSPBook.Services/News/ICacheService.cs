namespace FSPBook.Services.News
{
    public interface ICacheService
    {
        Task<IEnumerable<NewsArticle>> GetOrAddAsync(string cacheKey, Func<Task<IEnumerable<NewsArticle>>> fetchFunction, TimeSpan expiration);
    }
}
