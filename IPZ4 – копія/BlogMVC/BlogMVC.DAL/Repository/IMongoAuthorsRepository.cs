using BlogMVC.DAL.Models;

namespace BlogMVC.DAL.Repository
{
    public interface IMongoAuthorsRepository
    {
        AuthorMongo Add(AuthorMongo entity);

        void Update(AuthorMongo entity);

        void Delete(string? id);

        IEnumerable<AuthorMongo> GetAll();

        AuthorMongo GetById(object id);
    }
}
