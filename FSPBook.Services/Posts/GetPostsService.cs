using FSPBook.Data.DTOs;
using FSPBook.Data.Repositories;

namespace FSPBook.Services.Posts
{
    public class GetPostsResult
    {
        public List<PostDto>? Posts { get; set; }
        public int TotalPages { get; set; }
    }

    public class GetPostsService : IGetPostsService
    {
        private readonly IPostRepository _postRepository;

        public GetPostsService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        /// <summary>
        /// Fetches paginated posts
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<GetPostsResult> GetPostsAsync(int page, int pageSize)
        {
            var (posts, totalCount) = await _postRepository
                                     .GetPaginatedPostsAsync(page, pageSize);

            var paginatedPosts = posts.Select(post => new PostDto
            {
                Id = post.Id,
                Content = post.Content,
                AuthorName = post.Author.FullName,
                DateTimePosted = post.DateTimePosted,
                AuthorId = post.AuthorId
            })?.ToList();

            return new GetPostsResult
            {
                Posts = paginatedPosts,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };
        }
    }
}
