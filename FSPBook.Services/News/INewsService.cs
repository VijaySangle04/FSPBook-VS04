using System.Diagnostics.SymbolStore;

namespace FSPBook.Services.News
{
    public interface INewsService
    {
        Task<IEnumerable<NewsArticle>> GetTopHeadlinesAsync(int limit, bool clearCache = false);
    }
}