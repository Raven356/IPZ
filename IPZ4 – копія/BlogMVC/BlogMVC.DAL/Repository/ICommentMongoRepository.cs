using BlogMVC.DAL.Models;

namespace BlogMVC.DAL.Repository
{
    public interface ICommentMongoRepository
    {
        CommentMongo Add(CommentMongo entity);

        void Update(CommentMongo entity);

        void Delete(string id);

        IEnumerable<CommentMongo> GetAll();

        CommentMongo GetById(object id);
    }
}
