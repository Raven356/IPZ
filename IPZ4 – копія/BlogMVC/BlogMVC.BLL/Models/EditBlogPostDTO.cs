using System.ComponentModel.DataAnnotations;

namespace BlogMVC.BLL.Models
{
    public class EditBlogPostDTO
    {
        public int? Id { get; set; }

        public string Title { get; set; } = null!;

        public string Text { get; set; } = null!;

        public string? Tags { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int AuthorId { get; set; }

        public int? CategoryId { get; set; }
    }
}
