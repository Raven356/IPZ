
using Microsoft.AspNetCore.Http;

namespace BlogMVC.Models
{
    public class AuthorDTO
    {
        public int Id { get; set; }

        public string NickName { get; set; } = null!;

        public string? UserId { get; set; }

        public IFormFile Image { get; set; } = null!;

        public string? ImagePath { get; set; }
    }
}
