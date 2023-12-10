using BlogMVC.BLL.Models;
using BlogMVC.Models;

namespace BlogMVC.BLL.Services.TagsService
{
    public interface ITagsService
    {
        Task<IEnumerable<TagsDTO>> GetByBlogPostId(int? id);

        Task Create(IEnumerable<string> tags, int blogId);

        Task Update(IEnumerable<string> tags, int blogId);

        public Task<IEnumerable<BlogPostDTO>> GetByTag(string tag);
    }
}
