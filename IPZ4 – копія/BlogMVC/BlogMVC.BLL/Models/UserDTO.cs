using Microsoft.AspNetCore.Identity;

namespace BlogMVC.Models
{
    public class UserDTO : IdentityUser
    {
        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;
    }
}
