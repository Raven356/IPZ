using BlogMVC.BLL.Models;
using BlogMVC.BLL.Services.ControllersService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogMVC.Controllers
{
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService _commentsService;

        public CommentsController(ICommentsService commentsService)
        {
            _commentsService = commentsService;
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CommentDTO newComment)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(newComment.Text))
                {
                    newComment.UserId = GetUserId();
                    await _commentsService.AddNew(newComment);
                    return RedirectToAction("Details", "BlogPosts", new { id = newComment.BlogPostId });
                }

            }
            return RedirectToAction("Details", "BlogPosts");
        }
    }
}
