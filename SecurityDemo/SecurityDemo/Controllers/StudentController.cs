using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SecurityDemo.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Department()
        {
            var department =
                User.FindFirst("Department")?.Value;

            ViewBag.Department = department;

            return View();
        }
        [Authorize(Policy = "CSITOnly")]
        public IActionResult CSITOnly()
        {
            return View();
        }
    }
}