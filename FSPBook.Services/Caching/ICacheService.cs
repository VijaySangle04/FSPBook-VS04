namespace FSPBook.Services.Caching
{
    public interface ICacheService<T>
    {
        Task<IEnumerable<T>> GetOrAddAsync(string cacheKey,
            Func<Task<IEnumerable<T>>> fetchFunction, TimeSpan expiration);
    }
}
