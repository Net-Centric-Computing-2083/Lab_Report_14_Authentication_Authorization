using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SecurityDemo.Controllers
{
    public class AdminSetupController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AdminSetupController(
            UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> MakeAdmin()
        {
            var user = await _userManager.FindByEmailAsync(
                "admin@gmail.com"
            );

            if (user == null)
            {
                return Content("User not found.");
            }

            await _userManager.AddToRoleAsync(user, "Admin");

            return Content("Admin role assigned successfully.");
        }
    }
}