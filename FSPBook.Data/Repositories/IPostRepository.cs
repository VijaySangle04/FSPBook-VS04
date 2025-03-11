using FSPBook.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSPBook.Data.Repositories
{
    public interface IPostRepository
    {
        Task AddPostAsync(Post post);
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<Post> GetPostByIdAsync(int id);
        Task<IEnumerable<Post>> GetPostsByAuthorIdAsync(int authorId);
        Task<(IEnumerable<Post> Posts, int TotalCount)> GetPaginatedPostsAsync(int page, int pageSize);
    }
}
