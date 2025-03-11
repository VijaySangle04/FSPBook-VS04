using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSPBook.Services.News
{
    public interface ICacheService
    {
        Task<IEnumerable<NewsArticle>> GetOrAddAsync(string cacheKey, Func<Task<IEnumerable<NewsArticle>>> fetchFunction, TimeSpan expiration);
    }
}
