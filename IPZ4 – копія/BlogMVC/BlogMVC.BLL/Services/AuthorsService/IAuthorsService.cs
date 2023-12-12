using BlogMVC.Models;

namespace BlogMVC.BLL.Services.AuthorsService
{
    public interface IAuthorsService
    {
        Task<AuthorDTO> Create(AuthorDTO request);

        AuthorDTOMongo CreateMongo(AuthorDTOMongo request);

        Task<AuthorDTO> GetById(int? id);

        AuthorDTOMongo GetByIdMongo(string id);

        Task<AuthorDTO> GetByUser(string userId);

        AuthorDTOMongo GetByUserMongo(string userId);
    }
}
