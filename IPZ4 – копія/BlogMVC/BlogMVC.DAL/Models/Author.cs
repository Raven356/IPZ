using System.ComponentModel.DataAnnotations.Schema;

namespace BlogMVC.DAL.Models
{
    public class Author
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string NickName { get; set; } = null!;

        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        public User? User { get; set; }

        public override string? ToString()
        {
            return NickName;
        }
    }
}
