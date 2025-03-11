using FSPBook.Data.Entities;
using FSPBook.Data.Repositories;

namespace FSPBook.Services.Posts
{
    public class CreatePostService : ICreatePostService
    {
        private readonly IPostRepository _postRepository;

        public CreatePostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        /// <summary>
        /// Creates a new post
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<int> CreatePostAsync(int authorId, string content)
        {
            var post = new Post
            {
                AuthorId = authorId,
                Content = content,
                DateTimePosted = DateTimeOffset.Now
            };

            await _postRepository.AddPostAsync(post);
            return post.Id;
        }
    }
}
