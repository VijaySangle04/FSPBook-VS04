
namespace FSPBook.Services.Posts
{
    public interface IGetPostsService
    {
        Task<GetPostsResult> GetPostsAsync(int page, int pageSize);
    }
}