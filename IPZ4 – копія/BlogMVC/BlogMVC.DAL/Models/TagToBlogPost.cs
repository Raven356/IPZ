using System.ComponentModel.DataAnnotations.Schema;

namespace BlogMVC.DAL.Models
{
    public class TagToBlogPost
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int BlogPostId { get; set; }

        public int TagId { get; set; }

        [ForeignKey(nameof(BlogPostId))]
        public BlogPost? BlogPost { get; set; }

        [ForeignKey(nameof(TagId))]
        public Tags? Tags { get; set; }
    }
}
