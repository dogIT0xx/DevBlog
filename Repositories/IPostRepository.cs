using DevBlog.Entities;

namespace DevBlog.Repositories
{
    public interface IPostRepository
    {
        Task<List<Post>> GetListAsync(string? search, int? pageIndex);
        Task<List<Post>> GetListByCategoryAsync(string? category, int? pageIndex);
        Task<List<Post>> Get5LastPostsAsync();
        Task<Post> GetByIdAsync(int id);
        Task<int> DeleteleByIdAsync(int id);
        Task<int> EditAsync(Post post);
    }
}
