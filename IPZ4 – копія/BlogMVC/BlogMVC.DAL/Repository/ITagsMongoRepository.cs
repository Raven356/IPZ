using BlogMVC.DAL.Models;

namespace BlogMVC.DAL.Repository
{
    public interface ITagsMongoRepository
    {
        TagsMongo Add(TagsMongo entity);

        void Update(TagsMongo entity);

        void Delete(string? id);

        IEnumerable<TagsMongo> GetAll();

        TagsMongo GetById(object id);
    }
}
