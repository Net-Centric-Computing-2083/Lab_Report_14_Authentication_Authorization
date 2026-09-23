using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SecurityDemo.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        // Accessible only to logged-in users
        public IActionResult Index()
        {
            ViewBag.UserName = User.Identity?.Name;

            return View();
        }

        // Student role only
        [Authorize(Roles = "Student")]
        public IActionResult Dashboard()
        {
            ViewBag.UserName = User.Identity?.Name;

            return View();
        }

        // Display student's department claim
        [Authorize(Roles = "Student")]
        public IActionResult Department()
        {
            var department =
                User.FindFirst("Department")?.Value;

            ViewBag.Department = department;

            return View();
        }

        // CSIT claim/policy protected page
        [Authorize(Policy = "CSITOnly")]
        public IActionResult CSITPage()
        {
            return View();
        }
    }
}