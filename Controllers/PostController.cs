using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DevBlog.Data;
using DevBlog.Entities;
using DevBlog.Areas.Admin.Models.Post;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DevBlog.Models;
using HtmlAgilityPack;
using System.Web;

namespace DevBlog.Controllers
{

    public class PostController : Controller
    {
        private readonly DevBlogContext _context;
    
        public PostController(DevBlogContext context)
        {
            _context = context;
        }

        // GET: Post
        public async Task<IActionResult> Index()
        {
            var postModels = await _context.Posts
                 .Select(post => new PostModel
                 {
                     Id = post.Id,
                     Title = post.Title,
                     ThumbnailUrl = post.ThumbnailUrl,
                     PreviewContent = HandelPreviewContent(post.Content.ToString().Substring(0, 400))
                 })
                 .AsNoTracking()
                 .ToListAsync();
            return View(postModels);
        }

        // bỏ static bị lỗi, chưa tìm lí do
        static private string HandelPreviewContent(string htmlEncode)
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

        // GET: Post/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }
    }
}
