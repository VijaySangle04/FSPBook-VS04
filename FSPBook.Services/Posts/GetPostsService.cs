using FSPBook.Data.DTOs;
using FSPBook.Data.Repositories;

namespace FSPBook.Services.Posts
{
    public class GetPostsResult
    {
        public List<PostDto>? Posts { get; set; }
        public int TotalPages { get; set; }
    }

    public class GetPostsService
    {
        private readonly IPostRepository _postRepository;

        public GetPostsService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<GetPostsResult> GetPostsAsync(int page, int pageSize)
        {
            var (posts, totalCount) = await _postRepository.GetPaginatedPostsAsync(page, pageSize);

            var paginatedPosts = posts.Select(p => new PostDto
            {
                Id = p.Id,
                Content = p.Content,
                AuthorName = $"{p.Author.FirstName} {p.Author.LastName}",
                DateTimePosted = p.DateTimePosted,
                AuthorId = p.AuthorId
            }).ToList();

            return new GetPostsResult
            {
                Posts = paginatedPosts,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };
        }
    }
}
