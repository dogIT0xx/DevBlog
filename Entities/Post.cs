using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace DevBlog.Entities
{
    public class Post
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string ThumbnailUrl { get; set; }

        [ForeignKey(nameof(Author))]
        public string AuthorId { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Title { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Slug { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string Content { get; set; }

        public IdentityUser Author { get; set; }

        public List<PostMeta>? PostMetas { get; set; }

        public Category? Category { get; set; }
    }
}
