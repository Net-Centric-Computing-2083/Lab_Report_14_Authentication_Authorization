using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lab_Report_14_Authentication_Authorization.Controllers
{
    public class SecurityController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public SecurityController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return View();
        }

        [Authorize(Roles = "Student")]
        public IActionResult Student()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> MakeAdmin()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }

            return RedirectToAction("Admin");
        }

        [Authorize]
        public async Task<IActionResult> MakeStudent()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, "Student");
            }

            return RedirectToAction("Student");
        }

        [Authorize]
        public async Task<IActionResult> AddDepartmentClaim()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Challenge();

            var claims = await _userManager.GetClaimsAsync(user);

            if (!claims.Any(c => c.Type == "Department"))
            {
                await _userManager.AddClaimAsync(
                    user,
                    new Claim("Department", "CSIT"));
            }

            return RedirectToAction("CSITOnly");
        }

        [Authorize(Policy = "CSITOnly")]
        public async Task<IActionResult> CSITOnly()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Challenge();

            var claims = await _userManager.GetClaimsAsync(user);

            var department = claims
                .FirstOrDefault(c => c.Type == "Department")?.Value;

            ViewBag.Department = department;

            return View();
        }
        [AllowAnonymous]
        public IActionResult PublicPage()
        {
            return View();
        }
    }

}