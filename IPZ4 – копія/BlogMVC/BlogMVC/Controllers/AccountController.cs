using AutoMapper;
using BlogMVC.BLL.Models;
using BlogMVC.BLL.Services.AccountsService;
using BlogMVC.Models;
using Microsoft.AspNetCore.Mvc;

public class AccountController : BlogMVC.Controllers.ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IMapper _mapper;

    public AccountController(IAccountService accountService, IMapper mapper)
    {
        _accountService = accountService;
        _mapper = mapper;
    }

    
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = _mapper.Map<UserDTO>(model);
            var result = await _accountService.Register(user, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "BlogPosts");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return View(model);
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _accountService.Login(_mapper.Map<LoginDTO>(model));

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "BlogPosts");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt");
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _accountService.Logout();
        return RedirectToAction("Index", "BlogPosts");
    }
}
