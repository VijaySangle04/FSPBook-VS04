
namespace FSPBook.Services.Posts
{
    public interface ICreatePostService
    {
        Task<int> CreatePostAsync(int authorId, string content);
    }
}