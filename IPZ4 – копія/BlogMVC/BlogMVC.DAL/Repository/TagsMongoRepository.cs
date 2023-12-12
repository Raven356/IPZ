using BlogMVC.DAL.Context;
using BlogMVC.DAL.Models;
using MongoDB.Driver;

namespace BlogMVC.DAL.Repository
{
    public class TagsMongoRepository : ITagsMongoRepository
    {
        private readonly BlogMongoDBContext _dbContext;
        private readonly IMongoCollection<TagsMongo> _tags;
        private const string DB_NAME = "tags";

        public TagsMongoRepository(BlogMongoDBContext blogMongoDBContext)
        {
            _dbContext = blogMongoDBContext;
            _tags = _dbContext.GetCollection<TagsMongo>(DB_NAME);
        }
        public TagsMongo Add(TagsMongo entity)
        {
            _tags.InsertOne(entity);
            return entity;
        }

        public void Delete(string? id)
        {
            _tags.DeleteOne(a => a.Id == id);
        }

        public IEnumerable<TagsMongo> GetAll()
        {
            return _tags.Find(TagsMongo => true).ToList();
        }

        public TagsMongo GetById(object id)
        {
            return _tags.Find(a => a.Id == (string)id).First();
        }

        public void Update(TagsMongo entity)
        {
            _tags.ReplaceOne(a => a.Id == entity.Id, entity);
        }
    }
}
