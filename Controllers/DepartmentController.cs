using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab14.Controllers;

[Authorize(Policy = "CSITOnly")]
public class DepartmentController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
