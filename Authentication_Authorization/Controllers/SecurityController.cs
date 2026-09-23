using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication_Authorization.Controllers
{
    public class SecurityController : Controller
    {
        // ==================================================
        // DASHBOARD
        // ==================================================

        [Authorize]
        public IActionResult Dashboard()
        {
            return View();
        }


        // ==================================================
        // ADMIN PAGE
        // ==================================================

        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return View();
        }


        // ==================================================
        // STUDENT PAGE
        // ==================================================

        [Authorize(Roles = "Student")]
        public IActionResult Student()
        {
            return View();
        }


        // ==================================================
        // CSIT POLICY PAGE
        // ==================================================

        [Authorize(Policy = "CSITOnly")]
        public IActionResult CSIT()
        {
            var department =
                User.FindFirst("Department")?.Value;

            ViewBag.Department = department;

            return View();
        }


        // ==================================================
        // USER PROFILE
        // ==================================================

        [Authorize]
        public IActionResult Profile()
        {
            var email =
                User.Identity?.Name;

            var department =
                User.FindFirst("Department")?.Value;

            var role =
                User.FindFirst(ClaimTypes.Role)?.Value;

            ViewBag.Email = email;

            ViewBag.Department = department;

            ViewBag.Role = role;

            return View();
        }


        // ==================================================
        // CLAIM DETAILS
        // ==================================================

        [Authorize]
        public IActionResult Claims()
        {
            var claims = User.Claims.ToList();

            return View(claims);
        }


        // ==================================================
        // PUBLIC PAGE
        // ==================================================

        [AllowAnonymous]
        public IActionResult Public()
        {
            return View();
        }
    }
}