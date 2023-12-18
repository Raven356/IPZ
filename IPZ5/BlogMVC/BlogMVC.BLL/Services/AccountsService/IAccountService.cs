using BlogMVC.BLL.Models;
using BlogMVC.Models;
using Microsoft.AspNetCore.Identity;

namespace BlogMVC.BLL.Services.AccountsService
{
    public interface IAccountService
    {
        Task<IdentityResult> Register(UserDTO user, string password);

        Task<SignInResult> Login(LoginDTO model);

        Task Logout();
    }
}
