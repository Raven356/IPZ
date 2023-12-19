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
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public AuthorsService(IRepository<Author> repository, IRepository<User> userRepository, IMapper mapper)
        {
            _repository = repository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<AuthorDTO> Create(AuthorDTO request)
        {
            var author = _mapper.Map<Author>(request);
            if (request.Image != null)
            {
                var imagePath = Path.Combine("/app/data", request.Image.FileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await request.Image.CopyToAsync(stream);
                }
                author.Image = $"/app/data/{request.Image.FileName}";
            }
            return _mapper.Map<AuthorDTO>(await _repository.Add(author));
        }

        public async Task UpdateImage(AuthorDTO request)
        {
            var author = _repository.GetById(request.Id).Result; 
            var oldPath = author.Image;
            author.NickName = request.NickName;
            author.UserId = request.UserId;
            if (!string.IsNullOrEmpty(oldPath)) 
            { 
                File.Delete(oldPath);
            }
            if (request.Image != null)
            {
                var imagePath = Path.Combine("/app/data", request.Image.FileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await request.Image.CopyToAsync(stream);
                }
                author.Image = $"/app/data/{request.Image.FileName}";
            }
            await _repository.Update(author);
        }

        public async Task<AuthorDTO> GetById(int? id)
        {
            var author = await _repository.GetById(id);

            author.User = await _userRepository.GetById(author.UserId);

            var result = _mapper.Map<AuthorDTO>(author);
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
