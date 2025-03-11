using FSPBook.Data.Entities;
using FSPBook.Data.Repositories;

namespace FSPBook.Services.Posts
{
    public class CreatePostService
    {
        private readonly IPostRepository _postRepository;

        public CreatePostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<int> CreatePostAsync(int authorId, string content)
        {
            var post = new Post
            {
                AuthorId = authorId,
                Content = content,
                DateTimePosted = DateTimeOffset.Now
            };

            await _postRepository.AddAsync(post);
            return post.Id;
        }
    }
}
