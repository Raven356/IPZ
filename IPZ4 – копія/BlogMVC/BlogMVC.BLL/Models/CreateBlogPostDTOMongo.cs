using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlogMVC.BLL.Models
{
    public class CreateBlogPostDTOMongo
    {
        public string? Id { get; set; }

        public string Title { get; set; } = null!;

        public string Text { get; set; } = null!;

        [DisplayName("Category")]
        public string CategoryName { get; set; } = null!;

        public string? Tags { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string AuthorId { get; set; }

        public IFormFile? ImageFile { get; set; }

        public byte[]? Image { get; set; }
    }
}
