using BlogMVC.Models;

namespace BlogMVC.BLL.Models
{
    public class CommentDTO
    {
        public int Id { get; set; }

        public string Text { get; set; } = null!;

        public string? UserId { get; set; }

        public int BlogPostId { get; set; }

        public UserDTO? User { get; set; }
    }
}
