using BlogMVC.BLL.Models;
using BlogMVC.Models;

namespace BlogMVC.BLL.Services.TagsService
{
    public interface ITagsService
    {
        Task<IEnumerable<TagsDTO>> GetByBlogPostId(int? id);

        IEnumerable<TagsDTOMongo> GetByBlogPostIdMongo(string? id);

        IEnumerable<BlogPostDTOMongo> GetByTagMongo(string tag);

        void CreateMongo(IEnumerable<string> tags, string blogId);

        void UpdateMongo(IEnumerable<string> tags, string blogId);


        Task Create(IEnumerable<string> tags, int blogId);

        Task Update(IEnumerable<string> tags, int blogId);

        public Task<IEnumerable<BlogPostDTO>> GetByTag(string tag);
    }
}
