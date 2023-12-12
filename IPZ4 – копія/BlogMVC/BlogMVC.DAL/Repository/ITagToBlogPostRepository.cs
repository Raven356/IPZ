using BlogMVC.DAL.Models;

namespace BlogMVC.DAL.Repository
{
    public interface ITagToBlogPostRepository
    {
        TagToBlogPostMongo Add(TagToBlogPostMongo entity);

        void Update(TagToBlogPostMongo entity);

        void Delete(string? id);

        IEnumerable<TagToBlogPostMongo> GetAll();

        TagToBlogPostMongo GetById(object id);
    }
}
