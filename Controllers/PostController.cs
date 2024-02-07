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
using DevBlog.Utils;
using System.Drawing.Printing;
using DevBlog.Repositories;

namespace DevBlog.Controllers
{

    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly DevBlogContext _context;

        public PostController(IPostRepository postRepository, DevBlogContext context)
        {
            _postRepository = postRepository;
            _context = context;
        }

        // GET: Post
        public async Task<IActionResult> Index(int? pageNumber, string? searchString, string? sortName, string? sortOrder)
        {
            var posts = await _postRepository.GetListAsync(pageNumber);
            return View(posts);
        }


        [Route("{controller}/{action}/{category}")]
        public async Task<IActionResult> Category(string category, int? pageNumber)
        {
            return View();
        }

        // GET: Post/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }
    }
}
