using Microsoft.Extensions.Configuration;

namespace FSPBook.Services.News
{
    public class NewsApiClient : INewsApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public NewsApiClient(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> GetTopHeadlinesAsync(int limit)
        {
            var apiToken = _configuration["TheNewsApi:ApiToken"];
            var baseUrl = _configuration["TheNewsApi:BaseUrl"];
            var categories = _configuration["TheNewsApi:Categories"];
            var language = _configuration["TheNewsApi:Language"];
            var url = $"{baseUrl}?" +
                      $"api_token={apiToken}" +
                      $"&categories={categories}" +
                      $"&language={language}" +
            $"&limit={limit}";

            var client = _httpClientFactory.CreateClient();
            return await client.GetAsync(url);
        }
    }
}
