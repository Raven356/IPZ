using BlogMVC.Models;

namespace BlogMVC.BLL.Services.AuthorsService
{
    public interface IAuthorsService
    {
        Task<AuthorDTO> Create(AuthorDTO request);

        Task<AuthorDTO> GetById(int? id);

        Task<AuthorDTO> GetByUser(string userId);
    }
}
