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

        public async Task<IActionResult> Index()
        {
            var posts = await _postRepository.GetListAsync(null, null);
            return View(posts);
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
