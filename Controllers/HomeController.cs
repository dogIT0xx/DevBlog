using DevBlog.Data;
using DevBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using HtmlAgilityPack;
using System.Net;
using System.Web;
using DevBlog.Utils;
using DevBlog.Repositories;

namespace DevBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IPostRepository postRepository)
        {
            _postRepository = postRepository;
            _logger = logger;
        }
    
        public async Task<IActionResult> Index(int? pageNumber, string? searchString, string? sortName, string? sortOrder)
        {
            var posts = await _postRepository.GetListAsync(pageNumber);
            return View(posts);
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
