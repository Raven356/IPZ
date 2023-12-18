using BlogMVC.BLL.Models;
using System.ComponentModel.DataAnnotations;

namespace BlogMVC.Models
{
    public class BlogPostDTOMongo
    {
        public string? Id { get; set; }

        public string Title { get; set; } = null!;

        public string Text { get; set; } = null!;

        public string CategoryId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string AuthorId { get; set; }

        public AuthorDTOMongo? Author { get; set; }

        public CategoryDTOMongo? Category { get; set; }

        public byte[] Image { get; set; }

        public IEnumerable<TagsDTOMongo> Tags { get; set; } = Enumerable.Empty<TagsDTOMongo>();

        public IEnumerable<CommentDTOMongo> CommentList { get; set; } = null!;
    }
}
