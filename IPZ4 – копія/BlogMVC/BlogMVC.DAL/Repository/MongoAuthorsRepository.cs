using BlogMVC.DAL.Context;
using BlogMVC.DAL.Models;
using MongoDB.Driver;

namespace BlogMVC.DAL.Repository
{
    public class MongoAuthorsRepository : IMongoAuthorsRepository
    {
        private readonly BlogMongoDBContext _dbContext;
        private readonly IMongoCollection<AuthorMongo> _authors;
        private const string DB_NAME = "authors";

        public MongoAuthorsRepository(BlogMongoDBContext blogMongoDBContext) 
        { 
            _dbContext = blogMongoDBContext;
            _authors = _dbContext.GetCollection<AuthorMongo>(DB_NAME);
        }

        public AuthorMongo Add(AuthorMongo entity)
        {
            _authors.InsertOne(entity);
            return entity;
        }

        public void Delete(string? id)
        {
            _authors.DeleteOne(a => a.Id == id);
        }

        public IEnumerable<AuthorMongo> GetAll()
        {
            return _authors.Find(author => true).ToList();
        }

        public AuthorMongo GetById(object id)
        {
            return _authors.Find(a => a.Id == (string)id).First();
        }

        public void Update(AuthorMongo entity)
        {
            _authors.ReplaceOne(a => a.Id == entity.Id, entity);
        }
    }
}
