using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlogMVC.BLL.Models
{
    public class CreateBlogPostDTO
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
