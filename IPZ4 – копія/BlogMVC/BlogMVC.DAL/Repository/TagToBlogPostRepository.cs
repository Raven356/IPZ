using BlogMVC.DAL.Context;
using BlogMVC.DAL.Models;
using MongoDB.Driver;

namespace BlogMVC.DAL.Repository
{
    public class TagToBlogPostRepository : ITagToBlogPostRepository
    {
        private readonly BlogMongoDBContext _dbContext;
        private readonly IMongoCollection<TagToBlogPostMongo> _tagstoblogpost;
        private const string DB_NAME = "tagtoblogpost";

        public TagToBlogPostRepository(BlogMongoDBContext blogMongoDBContext)
        {
            _dbContext = blogMongoDBContext;
            _tagstoblogpost = _dbContext.GetCollection<TagToBlogPostMongo>(DB_NAME);
        }
        public TagToBlogPostMongo Add(TagToBlogPostMongo entity)
        {
            _tagstoblogpost.InsertOne(entity);
            return entity;
        }

        public void Delete(string? id)
        {
            _tagstoblogpost.DeleteOne(a => a.Id == id);
        }

        public IEnumerable<TagToBlogPostMongo> GetAll()
        {
            return _tagstoblogpost.Find(TagToBlogPostMongo => true).ToList();
        }

        public TagToBlogPostMongo GetById(object id)
        {
            return _tagstoblogpost.Find(a => a.Id == (string)id).First();
        }

        public void Update(TagToBlogPostMongo entity)
        {
            _tagstoblogpost.ReplaceOne(a => a.Id == entity.Id, entity);
        }
    }
}
