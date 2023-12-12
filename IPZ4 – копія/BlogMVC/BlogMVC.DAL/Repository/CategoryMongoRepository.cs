using BlogMVC.DAL.Context;
using BlogMVC.DAL.Models;
using MongoDB.Driver;

namespace BlogMVC.DAL.Repository
{
    public class CategoryMongoRepository : ICategoryMongoRepository
    {
        private readonly BlogMongoDBContext _dbContext;
        private readonly IMongoCollection<CategoryMongo> _categories;
        private const string DB_NAME = "categories";

        public CategoryMongoRepository(BlogMongoDBContext blogMongoDBContext)
        {
            _dbContext = blogMongoDBContext;
            _categories = _dbContext.GetCollection<CategoryMongo>(DB_NAME);
        }
        public CategoryMongo Add(CategoryMongo entity)
        {
            _categories.InsertOne(entity);
            return entity;
        }

        public void Delete(string? id)
        {
            _categories.DeleteOne(a => a.Id == id);
        }

        public IEnumerable<CategoryMongo> GetAll()
        {
            return _categories.Find(Category => true).ToList();
        }

        public CategoryMongo GetById(object id)
        {
            return _categories.Find(a => a.Id == (string)id).First();
        }

        public void Update(CategoryMongo entity)
        {
            _categories.ReplaceOne(a => a.Id == entity.Id, entity);
        }
    }
}
