using DevBlog.Entities;

namespace DevBlog.Repositories
{
    public interface IPostRepository
    {
        Task<List<Post>> GetListAsync(int? pageIndex);
        Task<List<Post>> GetListByCategoryIdAsync(int categoryId, int? pageIndex);
        Task<Post> GetByIdAsync(int id);
        Task DeleteleByIdAsync(int id);
        Task<bool> EditAsync(int id, Post post);
    }
}
