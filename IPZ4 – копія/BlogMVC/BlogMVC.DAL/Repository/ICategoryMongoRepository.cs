using BlogMVC.DAL.Models;

namespace BlogMVC.DAL.Repository
{
    public interface ICategoryMongoRepository
    {
        CategoryMongo Add(CategoryMongo entity);

        void Update(CategoryMongo entity);

        void Delete(string? id);

        IEnumerable<CategoryMongo> GetAll();

        CategoryMongo GetById(object id);
    }
}
