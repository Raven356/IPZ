using BlogMVC.BLL.Models;

namespace BlogMVC.Models
{
    public class BlogPostViewModelMongo
    {
        public BlogPostDTOMongo BlogPost { get; set; } = null!;

        public CommentDTOMongo NewComment { get; set; } = null!;

        public bool IsAuthor { get; set; } = default;
    }
}
