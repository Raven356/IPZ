using Microsoft.AspNetCore.Mvc;
using BlogMVC.BLL.Services.AuthorsService;
using BlogMVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace BlogMVC.Controllers
{
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorsService _authorsService;

        public AuthorsController(IAuthorsService authorsService)
        {
            _authorsService = authorsService;
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var author = await _authorsService.GetById(id);

            return View(author);
        }

        [HttpPost]
        public async Task<IActionResult> Details(AuthorDTO authorDTO)
        {
            await _authorsService.UpdateImage(authorDTO);

            return View(authorDTO);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorDTO author)
        {
            if (ModelState.IsValid)
            {
                var userId = GetUserId();
                author.UserId = userId;
                author = await _authorsService.Create(author);
                return RedirectToAction("Create", "BlogPosts", new { authorId = author.Id });
            }
            return View(author);
        }

        [Authorize]
        public async Task<IActionResult> CheckForAuthorExistence()
        {
            var author = await _authorsService.GetByUser(GetUserId());
            if (author == null)
            {
                return RedirectToAction("Create");
            }
            return RedirectToAction("Create", "BlogPosts", new { authorId = author.Id });
        }
    }
}
