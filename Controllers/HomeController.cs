﻿using DevBlog.Data;
using DevBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using HtmlAgilityPack;
using System.Net;
using System.Web;

namespace DevBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly DevBlogContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DevBlogContext context)
        {
            _context = context;
            _logger = logger;
        }
    
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
