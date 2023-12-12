using BlogMVC.DAL.Models;

namespace BlogMVC.DAL.Repository
{
    public interface IBlogPostMongoRepository
    {
        BlogPostMongo Add(BlogPostMongo entity);

        void Update(BlogPostMongo entity);

        void Delete(string? id);

        IEnumerable<BlogPostMongo> GetAll();

        BlogPostMongo GetById(object id);
    }
}
