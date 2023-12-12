using AutoMapper;
using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;
using BlogMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogMVC.BLL.Services.CategoriesService
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ICategoryMongoRepository _categoryMongoRepository;

        public CategoriesService(IRepository<Category> categoryRepository, IMapper mapper, ICategoryMongoRepository commentMongoRepository)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _categoryMongoRepository = commentMongoRepository;
        }

        public CategoryDTOMongo GetByIdMongo(string? id)
        {
            var category = _categoryMongoRepository.GetById(id);
            var result = _mapper.Map<CategoryDTOMongo>(category);
            return result;
        }

        public async Task<CategoryDTO> GetById(int? id)
        {
            var category = await _categoryRepository.GetAll()
                .FirstOrDefaultAsync(m => m.Id == id);
            var result = _mapper.Map<CategoryDTO>(category);
            return result;
        }
    }
}
