using Microsoft.AspNetCore.Identity;

namespace BlogMVC.DAL.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;
    }
}
