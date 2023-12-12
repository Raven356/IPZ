using BlogMVC.Models;

namespace BlogMVC.BLL.Models
{
    public class CommentDTOMongo
    {
        public string? Id { get; set; }

        public string Text { get; set; } = null!;

        public string? UserId { get; set; }

        public string BlogPostId { get; set; }

        public UserDTO? User { get; set; }
    }
}
