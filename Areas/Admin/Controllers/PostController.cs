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
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace DevBlog.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PostController : Controller
    {
        private readonly DevBlogContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICloudinary _cloudinary;

        public PostController(DevBlogContext context, UserManager<IdentityUser> userManager, ICloudinary cloudinary)
        {
            _context = context;
            _userManager = userManager;
            _cloudinary = cloudinary;
        }

        // GET: Post
        public async Task<IActionResult> Index()
        {
            var devBlogContext = _context.Posts.Include(p => p.Author);
            return View(await devBlogContext.ToListAsync());
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

        // GET: Post/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Post/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostModel createPostModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createPostModel);
            }

            var post = new Post()
            {
                AuthorId = "e14cf6d0-93b4-4bf5-8427-6089ced41daf",
                Title = createPostModel.Title,
                Content = createPostModel.Content,
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now,
                Slug = createPostModel.Title.Trim().ToLower().Replace(" ", "-")
            };

            // Xử lí url ảnh
            if (createPostModel.ThumbnailFile != null)
            {
                var stream = createPostModel.ThumbnailFile.OpenReadStream();
                Transformation[] listTrans = { 
                    new Transformation()
                            .Gravity("auto").Height(300).Width(400)
                            .Crop("thumb").Chain().Crop("crop"),
                   new Transformation()
                            .Gravity("auto").Height(600).Width(800)
                            .Crop("thumb").Chain().Crop("crop") };
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(createPostModel.FileName, stream),
                    Folder = "image",
                    PublicId = Guid.NewGuid().ToString(),
                    EagerTransforms = new List<Transformation>(listTrans)
                };

                var result = await _cloudinary.UploadAsync(uploadParams);
                // Set ThumbnailUrls
                post.ThumbnailUrl = result.Eager[0].Uri.ToString();
            }
            _context.Add(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Post/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", post.AuthorId);
            return View(post);
        }

        // POST: Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AuthorId,Title,Slug,CreateAt,UpdateAt,Content")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", post.AuthorId);
            return View(post);
        }

        // GET: Post/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
