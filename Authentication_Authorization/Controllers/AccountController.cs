using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SecurityDemo.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public IActionResult Profile()
        {
            ViewBag.Email = User.Identity?.Name;

            return View();
        }
    }
}