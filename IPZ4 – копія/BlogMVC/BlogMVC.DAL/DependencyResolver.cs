using BlogMVC.DAL.Context;
using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlogMVC.DAL
{
    public class DependencyResolver
    {
        public static void Configure(IServiceCollection services, string connectionString, string mongoConnection)
        {
            services.AddScoped<IBlogMVCContext, BlogMVCContext>();

            services.AddDbContext<BlogMVCContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddSingleton<BlogMongoDBContext>(provider =>
            {
                return new BlogMongoDBContext(mongoConnection, "blog");
            });

            services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<BlogMVCContext>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IMongoAuthorsRepository), typeof(MongoAuthorsRepository));
            services.AddScoped(typeof(IBlogPostMongoRepository), typeof(BlogPostMongoRepository));
            services.AddScoped(typeof(ICategoryMongoRepository), typeof(CategoryMongoRepository));
            services.AddScoped(typeof(ICommentMongoRepository), typeof(CommentMongoRepository));
            services.AddScoped(typeof(ITagsMongoRepository), typeof(TagsMongoRepository));
            services.AddScoped(typeof(ITagToBlogPostRepository), typeof(TagToBlogPostRepository));
        }
    }
}
