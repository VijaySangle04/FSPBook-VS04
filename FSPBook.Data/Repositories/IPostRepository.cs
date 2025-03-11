using FSPBook.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSPBook.Data.Repositories
{
    public interface IPostRepository
    {
        Task AddAsync(Post post);
        Task<IEnumerable<Post>> GetAllAsync();
        Task<Post> GetByIdAsync(int id);
        Task<IEnumerable<Post>> GetPostsByAuthorAsync(int authorId);
        Task<IEnumerable<Post>> GetPostsByAuthorIdAsync(int authorId);
        Task<(IEnumerable<Post> Posts, int TotalCount)> GetPaginatedPostsAsync(int page, int pageSize);
    }
}
