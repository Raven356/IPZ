using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogMVC.Controllers
{
    public class ControllerBase : Controller
    {
        public string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public override ViewResult View(object? model)
        {
            return base.View(model);
        }

        public override ViewResult View()
        {
            return base.View();
        }
    }
}
