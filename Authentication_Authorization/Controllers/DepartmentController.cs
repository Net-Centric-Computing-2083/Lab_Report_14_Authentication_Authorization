using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SecurityDemo.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        public IActionResult Index()
        {
            var department =
                User.FindFirst("Department")?.Value;

            ViewBag.Department = department;

            return View();
        }
    }
}