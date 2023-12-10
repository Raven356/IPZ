using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BlogMVC.Models
{
    public class EditBlogPostViewModel
    {
        public int? Id { get; set; }

        public string Title { get; set; } = null!;

        public string Text { get; set; } = null!;

        [DisplayName("Category")]
        public string CategoryName { get; set; } = null!;

        public string? Tags { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int AuthorId { get; set; }
    }
}
