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
        public static void Configure(IServiceCollection services, string connectionString)
        {
            services.AddScoped<IBlogMVCContext, BlogMVCContext>();

            services.AddDbContext<BlogMVCContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<BlogMVCContext>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}
