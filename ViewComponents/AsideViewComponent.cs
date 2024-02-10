using DevBlog.Data;
using DevBlog.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevBlog.ViewComponents
{
    public class AsideViewComponent : ViewComponent
    {
        private readonly IPostRepository _postRepository;
   
        public AsideViewComponent(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var posts = await _postRepository.Get5LastPostsAsync();
            return View("../Aside",posts);
        }
    }
}
