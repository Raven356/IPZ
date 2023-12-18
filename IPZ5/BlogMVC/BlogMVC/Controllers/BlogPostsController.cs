using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlogMVC.BLL.Models;
using BlogMVC.BLL.Services.BlogPostService;
using AutoMapper;
using System.Text;
using BlogMVC.Models;

namespace BlogMVC.Controllers
{
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostService _blogPostService;
        private readonly IMapper _mapper;

        public BlogPostsController(IBlogPostService blogPostService, IMapper mapper)
        {
            _blogPostService = blogPostService;
            _mapper = mapper;
        }

        // GET: BlogPosts
        public async Task<IActionResult> Index(string? searchTitle, string? searchCategory
            , string? searchAuthor, string? tagName)
        {
            var blogs = await _blogPostService.GetAll(
            new BlogPostSearchParametersDTO
            {
                SearchAuthor = searchAuthor
            ,
                SearchCategory = searchCategory
            ,
                SearchTitle = searchTitle
            });

            if (!string.IsNullOrEmpty(tagName))
            {
                blogs = await _blogPostService.GetByTag(tagName);
            }

            blogs = await _blogPostService.GetTags(blogs);
            return View(blogs);
        }

        // GET: BlogPosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var blogPost = await _blogPostService.GetById(id);
            
            var userId = GetUserId();

            var blogPostViewModel = new BlogPostViewModel 
            { 
                BlogPost = blogPost,
                IsAuthor = userId != null && blogPost.Author!.UserId!.Equals(userId)
            };

            return View(blogPostViewModel);
        }

        public async Task<IActionResult> GetImage(int id)
        {
            var blog = await _blogPostService.GetById(id);

            var bytes = blog.Image;

            if (bytes != null && bytes.Length > 0)
            {
                string contentType = GetContentType(bytes);
                return File(bytes, contentType);
            }
            else
            {
                // Return a default image or an empty image placeholder
                return NotFound(); // Replace with the path to your default image
            }
        }

        private string GetContentType(byte[] bytes)
        {
            if (IsJpeg(bytes))
            {
                return "image/jpeg";
            }
            else if (IsPng(bytes))
            {
                return "image/png";
            }
            else if (IsGif(bytes))
            {
                return "image/gif";
            }
            else
            {
                return "application/octet-stream"; // Fallback to a generic content type if the format is not recognized
            }
        }

        private bool IsJpeg(byte[] bytes)
        {
            return bytes.Length >= 2 &&
                   bytes[0] == 0xFF &&
                   bytes[1] == 0xD8;
        }

        private bool IsPng(byte[] bytes)
        {
            return bytes.Length >= 8 &&
                   bytes[0] == 0x89 &&
                   bytes[1] == 0x50 &&
                   bytes[2] == 0x4E &&
                   bytes[3] == 0x47 &&
                   bytes[4] == 0x0D &&
                   bytes[5] == 0x0A &&
                   bytes[6] == 0x1A &&
                   bytes[7] == 0x0A;
        }

        private bool IsGif(byte[] bytes)
        {
            return bytes.Length >= 6 &&
                   ((bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46 && bytes[3] == 0x38 && (bytes[4] == 0x37 || bytes[4] == 0x39) && bytes[5] == 0x61) ||
                    (bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46 && bytes[3] == 0x38 && (bytes[4] == 0x37 || bytes[4] == 0x39) && bytes[5] == 0x61));
        }

        // GET: BlogPosts/Create
        [Authorize]
        public IActionResult Create(int authorId)
        {
            return View(new CreateBlogPostDTO { AuthorId = authorId });
        }

        // POST: BlogPosts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBlogPostDTO blogPost)
        {
            if (ModelState.IsValid)
            {
                await _blogPostService.CreateNew(blogPost);
                return RedirectToAction("Index", "BlogPosts");
            }
            return View(blogPost);
        }

        // GET: BlogPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var response = await _blogPostService.GetById(id);

            var editBlogPostViewModel = _mapper.Map<EditBlogPostViewModel>(response);

            await CreateTagsString(editBlogPostViewModel, response.Tags);
            
            return View(editBlogPostViewModel);
        }

        // POST: BlogPosts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditBlogPostViewModel blogPost)
        {
            if (ModelState.IsValid)
            {
                var editBlogPostDTO = _mapper.Map<EditBlogPostDTO>(blogPost);

                await _blogPostService.Edit(editBlogPostDTO, blogPost.CategoryName);
               
                return RedirectToAction("Details", new { id = blogPost.Id });
            }

            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return View(await _blogPostService.GetById(id));
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _blogPostService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task CreateTagsString(EditBlogPostViewModel editBlogPost, IEnumerable<TagsDTO> tags)
        {
            var stringBuilder = new StringBuilder();
            foreach (var tag in tags)
            {
                stringBuilder.Append(tag.Name + " ");
            }
            editBlogPost.Tags = stringBuilder.ToString();
        }
    }
}
