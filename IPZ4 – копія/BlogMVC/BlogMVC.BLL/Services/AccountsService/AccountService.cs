using AutoMapper;
using BlogMVC.BLL.Models;
using BlogMVC.DAL.Models;
using BlogMVC.Models;
using Microsoft.AspNetCore.Identity;

namespace BlogMVC.BLL.Services.AccountsService
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public async Task<SignInResult> Login(LoginDTO model)
        {
            return await _signInManager
                .PasswordSignInAsync(model.Login, model.Password, isPersistent: false, lockoutOnFailure: false);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> Register(UserDTO user, string password)
        {
            var userModel = _mapper.Map<User>(user);
            var result = await _userManager.CreateAsync(userModel, password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(userModel, isPersistent: false);
            }
            return result;
        }
    }
}
