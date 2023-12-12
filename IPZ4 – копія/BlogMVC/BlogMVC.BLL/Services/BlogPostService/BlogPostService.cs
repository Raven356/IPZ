using AutoMapper;
using BlogMVC.BLL.Models;
using BlogMVC.BLL.Services.TagsService;
using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;
using BlogMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogMVC.BLL.Services.BlogPostService
{
    public class BlogPostService : IBlogPostService
    {
        private readonly IRepository<BlogPost> _blogPostRepository;
        private readonly IBlogPostMongoRepository _blogPostMongoRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly ICategoryMongoRepository _categoryMongoRepository;
        private readonly IRepository<Comment> _commentRepository;
        private readonly ICommentMongoRepository _commentMongoRepository;
        private readonly IRepository<Author> _authorRepository;
        private readonly IMongoAuthorsRepository _authorsRepository;
        private readonly IMapper _mapper;
        private readonly ITagsService _tagsService;

        public BlogPostService(
            ITagsService tagsService,
            IRepository<Category> categoryRepository,
            IRepository<Comment> commentRepository,
            IRepository<BlogPost> blogPostRepository,
            IRepository<Author> authorRepository,
            IMapper mapper,
            IBlogPostMongoRepository blogPostMongoRepository,
            ICategoryMongoRepository categoryMongoRepository,
            ICommentMongoRepository commentMongoRepository,
            IMongoAuthorsRepository authorsRepository)
        {
            _categoryRepository = categoryRepository;
            _commentRepository = commentRepository;
            _blogPostRepository = blogPostRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
            _tagsService = tagsService;
            _blogPostMongoRepository = blogPostMongoRepository;
            _categoryMongoRepository = categoryMongoRepository;
            _commentMongoRepository = commentMongoRepository;
            _authorsRepository = authorsRepository;
        }

        public async Task<BlogPostDTO> CreateNew(CreateBlogPostDTO request)
        {
            var newBlog = _mapper.Map<BlogPost>(request);
            newBlog.CategoryId = await GetCategoryId(request.CategoryName);
            var createdBlog = _mapper.Map<BlogPostDTO>(await _blogPostRepository.Add(newBlog));
            var tags = request.Tags == null ? null
                : request.Tags.Split(" ", StringSplitOptions.RemoveEmptyEntries).AsEnumerable();
            await _tagsService.Create(tags, createdBlog.Id);
            return createdBlog;
        }

        public BlogPostDTOMongo CreateNewMongo(CreateBlogPostDTOMongo request)
        {
            var newBlog = _mapper.Map<BlogPostMongo>(request);
            newBlog.CategoryId = GetCategoryIdMongo(request.CategoryName);
            var createdBlog = _mapper.Map<BlogPostDTOMongo>(_blogPostMongoRepository.Add(newBlog));
            var tags = request.Tags == null ? null
                : request.Tags.Split(" ", StringSplitOptions.RemoveEmptyEntries).AsEnumerable();
            _tagsService.CreateMongo(tags, createdBlog.Id);
            return createdBlog;
        }

        public async Task Delete(int request)
        {
            await _blogPostRepository.Delete(request);
        }

        public void DeleteMongo(string request)
        {
            _blogPostMongoRepository.Delete(request);
        }

        public async Task Edit(EditBlogPostDTO request, string categoryName)
        {
            request.CategoryId = await GetCategoryId(categoryName);
            var blog = _mapper.Map<BlogPost>(request);
            await _tagsService.Update(request.Tags == null ? null : request.Tags
                   .Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList()
                   , (int)blog.Id);
            await _blogPostRepository.Update(blog);
        }

        public void EditMongo(EditBlogPostDTOMongo request, string categoryName)
        {
            request.CategoryId = GetCategoryIdMongo(categoryName);
            var blog = _mapper.Map<BlogPostMongo>(request);
            _tagsService.UpdateMongo(request.Tags == null ? null : request.Tags
                   .Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList()
                   , (string)blog.Id);
            _blogPostMongoRepository.Update(blog);
        }

        public IEnumerable<BlogPostDTOMongo> GetAllMongo(BlogPostSearchParametersDTO request)
        {
            var blogs = _blogPostMongoRepository.GetAll()
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchTitle))
            {
                blogs = blogs.Where(b => b.Title.Contains(request.SearchTitle));
            }

            if (!string.IsNullOrEmpty(request.SearchCategory))
            {
                var category = _categoryMongoRepository.GetAll();
                blogs = blogs.Where(b => category.Where(c => c.Name.Contains(request.SearchCategory)).Select(c => c.Id).Contains(b.CategoryId));
            }

            if (!string.IsNullOrEmpty(request.SearchAuthor))
            {
                var author = _authorsRepository.GetAll();
                blogs = blogs.Where(b => author.Where(a => a.NickName.Contains(request.SearchAuthor)).Select(a => a.Id).Contains(b.AuthorId));
            }
            var result = _mapper.Map<IEnumerable<BlogPostDTOMongo>>(blogs);
            foreach (var blogPost in result)
            {
                blogPost.Author = _mapper.Map<AuthorDTOMongo>(_authorsRepository.GetById(blogPost.AuthorId));
                blogPost.Category = _mapper.Map<CategoryDTOMongo>(_categoryMongoRepository.GetById(blogPost.CategoryId));
            }
            return result;
        }

        public async Task<IEnumerable<BlogPostDTO>> GetAll(BlogPostSearchParametersDTO request)
        {
            var blogs = _blogPostRepository.GetAll()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchTitle))
            {
                blogs = blogs.Where(b => b.Title.Contains(request.SearchTitle));
            }

            if (!string.IsNullOrEmpty(request.SearchCategory))
            {
                var category = _categoryRepository.GetAll();
                blogs = blogs.Where(b => b.Category.Name.Contains(request.SearchCategory));
            }

            if (!string.IsNullOrEmpty(request.SearchAuthor))
            {
                var author = _authorRepository.GetAll();
                blogs = blogs.Where(b => b.Author.NickName.Contains(request.SearchAuthor));
            }
            var result = _mapper.Map<IEnumerable<BlogPostDTO>>(blogs);
            return result;
        }

        public BlogPostDTOMongo GetByIdMongo(string? id)
        {
            var blogPost = _mapper.Map<BlogPostDTOMongo>(_blogPostMongoRepository.GetById(id));

            var comments = _mapper
                .Map<IEnumerable<CommentDTOMongo>>(_commentMongoRepository.GetAll()
                .Where(c => c.BlogPostId == blogPost.Id).AsEnumerable());

            blogPost.CommentList = comments;
            blogPost.Author = _mapper.Map<AuthorDTOMongo>(_authorsRepository.GetById(blogPost.AuthorId));
            blogPost.Category = _mapper.Map<CategoryDTOMongo>(_categoryMongoRepository.GetById(blogPost.CategoryId));
            blogPost.Tags = _tagsService.GetByBlogPostIdMongo(blogPost.Id);
            return blogPost;
        }

        public async Task<BlogPostDTO> GetById(int? id)
        {
            var blogPost = _mapper.Map<BlogPostDTO>(await _blogPostRepository.GetAll()
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id));

            var comments = _mapper
                .Map<IEnumerable<CommentDTO>>(_commentRepository.GetAll().Include(c => c.User)
                .Where(c => c.BlogPostId == id).AsEnumerable());
            
            blogPost.CommentList = comments;
            blogPost.Tags = await _tagsService.GetByBlogPostId(blogPost.Id);
            return blogPost;
        }

        public async Task<IEnumerable<BlogPostDTO>> GetByTag(string tag)
        {
            return await _tagsService.GetByTag(tag);
        }

        public IEnumerable<BlogPostDTOMongo> GetByTagMongo(string tag)
        {
            var result = _tagsService.GetByTagMongo(tag);
            foreach (var x in result)
            {
                x.Author = _mapper.Map<AuthorDTOMongo>(_authorsRepository.GetById(x.AuthorId));
                x.Category = _mapper.Map<CategoryDTOMongo>(_categoryMongoRepository.GetById(x.CategoryId));
            }
            return result;
        }

        public async Task<IEnumerable<BlogPostDTO>> 
            GetTags(IEnumerable<BlogPostDTO> posts)
        {
            foreach (var blog in posts)
            {
                blog.Tags = await _tagsService.GetByBlogPostId(blog.Id);
            }
            return posts;
        }

        public IEnumerable<BlogPostDTOMongo> GetTagsMongo(IEnumerable<BlogPostDTOMongo> posts)
        {
            foreach (var blog in posts)
            {
                blog.Tags = _tagsService.GetByBlogPostIdMongo(blog.Id);
            }
            return posts;
        }

        private async Task<int> GetCategoryId(string categoryName)
        {
            int categoryId = -1;
            var category = _categoryRepository.GetAll()
                .Where(c => c.Name == categoryName)
                .FirstOrDefault();

            if (category == null)
            {
                categoryId = (await _categoryRepository.Add(new Category { Name = categoryName })).Id;
            }
            else
            {
                categoryId = category.Id;
            }
            return categoryId;
        }

        private string GetCategoryIdMongo(string categoryName)
        {
            string categoryId = "";
            var category = _categoryMongoRepository.GetAll()
                .Where(c => c.Name == categoryName)
                .FirstOrDefault();

            if (category == null)
            {
                categoryId = _categoryMongoRepository.Add(new CategoryMongo { Name = categoryName }).Id;
            }
            else
            {
                categoryId = category.Id;
            }
            return categoryId;
        }

    }
}
