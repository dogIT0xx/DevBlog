using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using DevBlog.Entities;
using Azure;

namespace DevBlog.Data;

public class DevBlogContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostMeta> PostMetas { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<PostCategory> PostCategories { get; set; }

    public DevBlogContext(DbContextOptions<DevBlogContext> options)
        : base(options)
    { 
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        
        base.OnModelCreating(builder);

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (tableName.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName.Substring(6));
            }
        }
    }
}
