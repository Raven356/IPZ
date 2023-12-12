using BlogMVC.DAL.Context;
using BlogMVC.DAL.Models;
using MongoDB.Driver;

namespace BlogMVC.DAL.Repository
{
    public class BlogPostMongoRepository : IBlogPostMongoRepository
    {
        private readonly BlogMongoDBContext _dbContext;
        private readonly IMongoCollection<BlogPostMongo> _posts;
        private const string DB_NAME = "blogposts";

        public BlogPostMongoRepository(BlogMongoDBContext blogMongoDBContext)
        {
            _dbContext = blogMongoDBContext;
            _posts = _dbContext.GetCollection<BlogPostMongo>(DB_NAME);
        }

        public BlogPostMongo Add(BlogPostMongo entity)
        {
            _posts.InsertOne(entity);
            return entity;
        }

        public void Delete(string? id)
        {
            _posts.DeleteOne(a => a.Id == id);
        }

        public IEnumerable<BlogPostMongo> GetAll()
        {
            return _posts.Find(BlogPostMongo => true).ToList();
        }

        public BlogPostMongo GetById(object id)
        {
            return _posts.Find(a => a.Id == (string)id).First();
        }

        public void Update(BlogPostMongo entity)
        {
            _posts.ReplaceOne(a => a.Id == entity.Id, entity);
        }
    }
}
