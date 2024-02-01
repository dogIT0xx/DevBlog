using Azure;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevBlog.Entities
{
    [PrimaryKey(nameof(PostId), nameof(CategoryId))]
    public class PostCategory
    {
        public int PostId { get; set; }

        public int CategoryId { get; set; }
    }
}
