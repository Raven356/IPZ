using BlogMVC.BLL.Models;

namespace BlogMVC.Models
{
    public class BlogPostViewModel
    {
        public BlogPostDTO BlogPost { get; set; } = null!;

        public CommentDTO NewComment { get; set; } = null!;

        public bool IsAuthor { get; set; } = default;
    }
}
