using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BlogMVC.DAL.Models;

namespace BlogMVC.DAL.Context
{
    public class BlogMVCContext : IdentityDbContext<User>, IBlogMVCContext
    {
        public BlogMVCContext (DbContextOptions<BlogMVCContext> options)
            : base(options)
        {
        }

        public DbSet<Author> Author { get; set; } = default!;

        public DbSet<BlogPost>? BlogPost { get; set; }

        public DbSet<Category>? Category { get; set; }

        public DbSet<Comment>? Comment { get; set; }

        public DbSet<User>? User { get; set; }

        public DbSet<Tags>? Tags { get; set; }

        public DbSet<TagToBlogPost>? TagToBlogPosts { get; set; }
    }
}
