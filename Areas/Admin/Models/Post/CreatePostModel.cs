using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace DevBlog.Areas.Admin.Models.Post
{
    public class CreatePostModel
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Choose an image")]
        public IFormFile ThumbnailFile { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [FileExtensions(Extensions = "jpg,jpeg,png")]
        public string FileName => ThumbnailFile.FileName;
    }
}
