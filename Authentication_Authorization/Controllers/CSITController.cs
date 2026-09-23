using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SecurityDemo.Controllers
{
    [Authorize(Policy = "CSITOnly")]
    public class CSITController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}