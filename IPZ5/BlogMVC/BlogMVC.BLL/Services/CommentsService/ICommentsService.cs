using BlogMVC.BLL.Models;

namespace BlogMVC.BLL.Services.ControllersService
{
    public interface ICommentsService
    {
        public Task AddNew(CommentDTO newComment);
    }
}
