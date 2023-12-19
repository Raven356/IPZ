using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogMVC.DAL.Models
{
    public class BlogPost
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Text { get; set; } = null!;

        public byte[]? Image { get; set; }

        public int CategoryId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public Author? Author { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }

    }
}
