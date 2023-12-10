using System.ComponentModel.DataAnnotations.Schema;

namespace BlogMVC.DAL.Models
{
    public class Comment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Text { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public int BlogPostId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [ForeignKey("BlogPostId")]
        public BlogPost? BlogPost { get; set; }
    }
}
