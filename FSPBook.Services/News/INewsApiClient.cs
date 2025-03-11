namespace FSPBook.Services.News
{
    public interface INewsApiClient
    {
        Task<HttpResponseMessage> GetTopHeadlinesAsync(int limit);
    }
}
