using DevBlog.Data;
using DevBlog.Entities;
using DevBlog.Models;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using System.Web;
using DevBlog.Utils;
using Microsoft.AspNetCore.Identity;

namespace DevBlog.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DevBlogContext _context;

        public PostRepository(DevBlogContext context)
        {
            _context = context;
        }

        public Task DeleteleByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditAsync(int id, Post post)
        {
            throw new NotImplementedException();
        }

        // Chưa tối ưu query
        public async Task<Post> GetByIdAsync(int id)
        {
            var post = await _context.Posts
                .Include(post => post.Author)
                .AsNoTracking()
                .SingleOrDefaultAsync(post => post.Id == id);
            return post;
        }

        public async Task<List<Post>> GetListByCategoryIdAsync(int categoryId, int? pageIndex)
        {
            var queryable = _context.Posts
                .Select(post => new Post
                {
                    Id = post.Id,
                    Title = post.Title,
                    ThumbnailUrl = post.ThumbnailUrl,
                    Content = HandelPreviewContent(post.Content.ToString().Substring(0, 400)),
                    Category = post.Category,
                    CreateAt = post.CreateAt
                })
                .Where(post => post.Category.Id == categoryId)
                .AsNoTracking();

            var pageSize = 4;
            var listPageinated = await PaginatedList<Post>.CreateAsync(queryable, pageIndex ?? 1, pageSize);

            return listPageinated;
        }

        public async Task<List<Post>> GetListAsync(int? pageIndex)
        {
            var queryable = _context.Posts
            .Select(post => new Post
            {
                Id = post.Id,
                Title = post.Title,
                ThumbnailUrl = post.ThumbnailUrl,
                Content = HandelPreviewContent(post.Content.ToString().Substring(0, 400)),
                Category = post.Category,
                CreateAt = post.CreateAt
            })
            .AsNoTracking();

            var pageSize = 4;
            var listPageinated = await PaginatedList<Post>.CreateAsync(queryable, pageIndex ?? 1, pageSize);

            return listPageinated;
        }

        // bỏ static bị lỗi, chưa tìm lí do
        private static string HandelPreviewContent(string htmlEncode)
        {
            // Decode html từ database
            var decodedContent = HttpUtility.HtmlDecode(htmlEncode);
            var htmlDoc = new HtmlDocument();
            // Load html từ decode
            htmlDoc.LoadHtml(decodedContent);
            // Chọn thẻ p tag
            var htmlPTagNode = htmlDoc.DocumentNode.SelectSingleNode("//p");
            // Lấy nội dung bên trong
            return htmlPTagNode.InnerText;
        }
    }
}
