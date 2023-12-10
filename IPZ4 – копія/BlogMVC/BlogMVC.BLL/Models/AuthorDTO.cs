
namespace BlogMVC.Models
{
    public class AuthorDTO
    {
        public int Id { get; set; }

        public string NickName { get; set; } = null!;

        public string? UserId { get; set; }
    }
}
