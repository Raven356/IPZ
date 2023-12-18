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

        public async Task<IActionResult> IndexMongo(string? searchTitle, string? searchCategory
            , string? searchAuthor, string? tagName)
        {
            var blogs = _blogPostService.GetAllMongo(
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
                blogs = _blogPostService.GetByTagMongo(tagName);
            }

            blogs = _blogPostService.GetTagsMongo(blogs);
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

        // GET: BlogPosts/DetailsMongo/5
        public async Task<IActionResult> DetailsMongo(string? id)
        {
            var blogPost = _blogPostService.GetByIdMongo(id);

            var userId = GetUserId();

            foreach (var c in blogPost.CommentList)
            {
                c.UserId = GetUserId();
            }

            var blogPostViewModel = new BlogPostViewModelMongo
            {
                BlogPost = blogPost,
                IsAuthor = userId != null && blogPost.Author!.UserId!.Equals(userId)
            };

            return View(blogPostViewModel);
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

        [Authorize]
        public IActionResult CreateMongo(string authorId)
        {
            return View(new CreateBlogPostDTOMongo { AuthorId = authorId });
        }

        // POST: BlogPosts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMongo(CreateBlogPostDTOMongo blogPost)
        {
            if (ModelState.IsValid)
            {
                if (blogPost.ImageFile != null && blogPost.ImageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await blogPost.ImageFile.CopyToAsync(memoryStream);
                        // Convert the image to a byte array
                        blogPost.Image = memoryStream.ToArray();
                    }
                }
                _blogPostService.CreateNewMongo(blogPost);
                return RedirectToAction("IndexMongo", "BlogPosts");
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

        // GET: BlogPosts/Edit/5
        public async Task<IActionResult> EditMongo(string? id)
        {
            var response = _blogPostService.GetByIdMongo(id);

            var editBlogPostViewModel = _mapper.Map<EditBlogPostViewModelMongo>(response);

            await CreateTagsStringMongo(editBlogPostViewModel, response.Tags);

            return View(editBlogPostViewModel);
        }

        // POST: BlogPosts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMongo(EditBlogPostViewModelMongo blogPost)
        {
            if (ModelState.IsValid)
            {
                if (blogPost.ImageFile != null && blogPost.ImageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await blogPost.ImageFile.CopyToAsync(memoryStream);
                        // Convert the image to a byte array
                        blogPost.Image = memoryStream.ToArray();
                    }
                }
                var editBlogPostDTO = _mapper.Map<EditBlogPostDTOMongo>(blogPost);

                _blogPostService.EditMongo(editBlogPostDTO, blogPost.CategoryName);

                return RedirectToAction("DetailsMongo", new { id = blogPost.Id });
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

        // GET: BlogPosts/Delete/5
        public async Task<IActionResult> DeleteMongo(string? id)
        {
            return View(_blogPostService.GetByIdMongo(id));
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("DeleteMongo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedMongo(string id)
        {
            _blogPostService.DeleteMongo(id);
            return RedirectToAction(nameof(IndexMongo));
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

        private async Task CreateTagsStringMongo(EditBlogPostViewModelMongo editBlogPost, IEnumerable<TagsDTOMongo> tags)
        {
            var stringBuilder = new StringBuilder();
            foreach (var tag in tags)
            {
                stringBuilder.Append(tag.Name + " ");
            }
            editBlogPost.Tags = stringBuilder.ToString();
        }

        public IActionResult GetImage(byte[] imageBytes)
        {

            if (imageBytes != null && imageBytes.Length > 0)
            {
                return File(imageBytes, "image/jpeg"); // Adjust the content type based on your image format
            }
            return BadRequest("No image found!");
            //else
            //{
            //    // Return a default image or an empty image placeholder
            //    //return File("~/path/to/default-image.jpg", "image/jpeg"); // Replace with the path to your default image
            //}
        }
    }
}
