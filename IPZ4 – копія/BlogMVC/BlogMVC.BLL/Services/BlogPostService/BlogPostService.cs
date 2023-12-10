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
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Comment> _commentRepository;
        private readonly IRepository<Author> _authorRepository;
        private readonly IMapper _mapper;
        private readonly ITagsService _tagsService;

        public BlogPostService(
            ITagsService tagsService,
            IRepository<Category> categoryRepository,
            IRepository<Comment> commentRepository,
            IRepository<BlogPost> blogPostRepository,
            IRepository<Author> authorRepository,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _commentRepository = commentRepository;
            _blogPostRepository = blogPostRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
            _tagsService = tagsService;
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

        public async Task Delete(int request)
        {
            await _blogPostRepository.Delete(request);
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

        public async Task<IEnumerable<BlogPostDTO>> 
            GetTags(IEnumerable<BlogPostDTO> posts)
        {
            foreach (var blog in posts)
            {
                blog.Tags = await _tagsService.GetByBlogPostId(blog.Id);
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

    }
}
