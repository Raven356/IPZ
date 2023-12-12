using AutoMapper;
using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;
using BlogMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogMVC.BLL.Services.AuthorsService
{
    public class AuthorsService : IAuthorsService
    {
        private readonly IRepository<Author> _repository;
        private readonly IMongoAuthorsRepository _authorsRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public AuthorsService(IRepository<Author> repository, IRepository<User> userRepository, IMapper mapper, IMongoAuthorsRepository authorsRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
            _mapper = mapper;
            _authorsRepository = authorsRepository;
        }

        public async Task<AuthorDTO> Create(AuthorDTO request)
        {
            var author = _mapper.Map<Author>(request);
            return _mapper.Map<AuthorDTO>(await _repository.Add(author));
        }

        public AuthorDTOMongo CreateMongo(AuthorDTOMongo request)
        {
            var author = _mapper.Map<AuthorMongo>(request);
            return _mapper.Map<AuthorDTOMongo>(_authorsRepository.Add(author));
        }

        public AuthorDTOMongo GetByIdMongo(string id)
        {
            var author = _authorsRepository.GetById(id);
            var result = _mapper.Map<AuthorDTOMongo>(author);
            return result;
        }

        public async Task<AuthorDTO> GetById(int? id)
        {
            var author = await _repository.GetById(id);

            author.User = await _userRepository.GetById(author.UserId);

            var result = _mapper.Map<AuthorDTO>(author);
            return result;
        }

        public AuthorDTOMongo GetByUserMongo(string userId)
        {
            var author = _authorsRepository.GetAll().FirstOrDefault(a => a.UserId!.Equals(userId));
            var result = _mapper.Map<AuthorDTOMongo>(author);
            return result;
        }

        public async Task<AuthorDTO> GetByUser(string userId)
        {
            var author = await _repository.GetAll().
                FirstOrDefaultAsync(a => a.UserId!.Equals(userId));
            var result = _mapper.Map<AuthorDTO>(author);
            return result;
        }
    }
}
