using BlogMVC.BLL.Models;
using System.ComponentModel.DataAnnotations;

namespace BlogMVC.Models
{
    public class BlogPostDTO
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Text { get; set; } = null!;

        public int CategoryId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int AuthorId { get; set; }

        public AuthorDTO? Author { get; set; }

        public CategoryDTO? Category { get; set; }

        public IEnumerable<TagsDTO> Tags { get; set; } = Enumerable.Empty<TagsDTO>();

        public IEnumerable<CommentDTO> CommentList { get; set; } = null!;
    }
}
