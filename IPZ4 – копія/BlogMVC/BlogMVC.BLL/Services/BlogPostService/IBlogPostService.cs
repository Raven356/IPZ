using BlogMVC.BLL.Models;
using BlogMVC.Models;

namespace BlogMVC.BLL.Services.BlogPostService
{
    public interface IBlogPostService
    {
        Task<BlogPostDTO> CreateNew(CreateBlogPostDTO request);

        BlogPostDTOMongo CreateNewMongo(CreateBlogPostDTOMongo request);

        IEnumerable<BlogPostDTOMongo> GetAllMongo(BlogPostSearchParametersDTO request);

        BlogPostDTOMongo GetByIdMongo(string? id);

        void DeleteMongo(string request);

        IEnumerable<BlogPostDTOMongo> GetByTagMongo(string tag);

        IEnumerable<BlogPostDTOMongo> GetTagsMongo(IEnumerable<BlogPostDTOMongo> posts);

        void EditMongo(EditBlogPostDTOMongo request, string categoryName);

        Task Delete(int id);

        Task Edit(EditBlogPostDTO request, string categoryName);

        Task<IEnumerable<BlogPostDTO>> GetAll(BlogPostSearchParametersDTO request);

        Task<BlogPostDTO> GetById(int? id);

        Task<IEnumerable<BlogPostDTO>> GetTags(IEnumerable<BlogPostDTO> posts);

        Task<IEnumerable<BlogPostDTO>> GetByTag(string tag);
    }
}
