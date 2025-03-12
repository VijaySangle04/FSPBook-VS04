using FSPBook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FSPBook.Data.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly Context _context;

        public PostRepository(Context context)
        {
            _context = context;
        }

        public async Task AddPostAsync(Post post)
        {
            await _context.Post.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            if(_context.Post == null || _context.Post.Count() == 0)
            {
                return null;
            }
            return await _context.Post
                .Include(p => p.Author)
                .ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            if (_context.Post == null || _context.Post.Count() == 0)
            {
                return null;
            }
            Post post = await _context.Post
                            .Include(p => p.Author)
                            .FirstOrDefaultAsync(p => p.Id == id);
            return post;
        }

        public async Task<IEnumerable<Post>> GetPostsByAuthorIdAsync(int authorId)
        {
            if (_context.Post == null || _context.Post.Count() == 0)
            {
                return null;
            }
            return await _context.Post
                .Where(p => p.AuthorId == authorId)
                .OrderByDescending(p => p.DateTimePosted)
                .Select(p => new Post
                {
                    Id = p.Id,
                    Content = p.Content,
                    DateTimePosted = p.DateTimePosted,
                    Author = p.Author,
                    AuthorId = authorId
                })
                .ToListAsync();
        }

        public async Task<(IEnumerable<Post> Posts, int TotalCount)> 
            GetPaginatedPostsAsync(int page, int pageSize)
        {
            if (_context.Post == null || _context.Post.Count() == 0)
            {
                return (null, 0);
            }
            var query = _context.Post.Include(p => p.Author)
                        .OrderByDescending(p => p.DateTimePosted);
            var totalCount = await query.CountAsync();
            var posts = await query.Skip((page - 1) * pageSize)
                        .Take(pageSize).ToListAsync();

            return (posts, totalCount);
        }
    }
}
