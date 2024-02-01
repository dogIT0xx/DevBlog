using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevBlog.Entities
{
    public class PostMeta
    {
        public Guid Id { get; set; }

        public int PostId { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Content { get; set; }
    }
}
