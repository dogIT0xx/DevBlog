using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevBlog.Entities
{
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(ParentCategory))]
        public int? ParentId { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Title { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Slug { get; set; }

        public Category? ParentCategory { get; set; }
    }
}
