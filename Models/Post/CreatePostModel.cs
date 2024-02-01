using System.ComponentModel.DataAnnotations;

namespace DevBlog.Models.Post
{
    public class CreatePostModel
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
