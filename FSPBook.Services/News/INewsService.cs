namespace FSPBook.Services.News
{
    public interface INewsService
    {
        Task<IEnumerable<NewsArticle>> GetTopHeadlinesAsync(int limit);
    }
}