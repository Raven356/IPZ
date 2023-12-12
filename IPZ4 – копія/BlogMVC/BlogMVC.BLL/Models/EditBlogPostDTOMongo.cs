using System.ComponentModel.DataAnnotations;

namespace BlogMVC.BLL.Models
{
    public class EditBlogPostDTOMongo
    {
        public string? Id { get; set; }

        public string Title { get; set; } = null!;

        public string Text { get; set; } = null!;

        public string? Tags { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string AuthorId { get; set; }

        public string? CategoryId { get; set; }
    }
}
