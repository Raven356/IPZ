using BlogMVC.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BlogMVC.DAL.Context
{
    public interface IBlogMVCContext : IDisposable
    {
        public DbSet<Author> Author { get; set; }

        public DbSet<BlogPost> BlogPost { get; set; }

        public DbSet<Category> Category { get; set; }

        public DbSet<Comment> Comment { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<Tags> Tags { get; set; }

        public DbSet<TagToBlogPost> TagToBlogPosts { get; set; }

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
