using BlogMVC.DAL.Context;
using BlogMVC.DAL.Models;
using MongoDB.Driver;

namespace BlogMVC.DAL.Repository
{
    public class CommentMongoRepository : ICommentMongoRepository
    {
        private readonly BlogMongoDBContext _dbContext;
        private readonly IMongoCollection<CommentMongo> _comments;
        private const string DB_NAME = "comments";

        public CommentMongoRepository(BlogMongoDBContext blogMongoDBContext)
        {
            _dbContext = blogMongoDBContext;
            _comments = _dbContext.GetCollection<CommentMongo>(DB_NAME);
        }
        public CommentMongo Add(CommentMongo entity)
        {
            _comments.InsertOne(entity);
            return entity;
        }

        public void Delete(string? id)
        {
            _comments.DeleteOne(a => a.Id == id);
        }

        public IEnumerable<CommentMongo> GetAll()
        {
            return _comments.Find(CommentMongo => true).ToList();
        }

        public CommentMongo GetById(object id)
        {
            return _comments.Find(a => a.Id == (string)id).First();
        }

        public void Update(CommentMongo entity)
        {
            _comments.ReplaceOne(a => a.Id == entity.Id, entity);
        }
    }
}
