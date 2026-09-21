using Lab14.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lab14.Controllers;

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
        var user =
            await _userManager.FindByEmailAsync("admin@lab14.com");

        if (user == null)
        {
            return NotFound("User not found.");
        }

        if (!await _userManager.IsInRoleAsync(user, "Admin"))
        {
            await _userManager.AddToRoleAsync(user, "Admin");
        }

        return Content("User is now an Admin.");
    }
    public async Task<IActionResult> MakeStudent()
    {
    var user =
        await _userManager.FindByEmailAsync("student@lab14.com");

    if (user == null)
    {
        return NotFound("Student user not found.");
    }

    if (!await _userManager.IsInRoleAsync(user, "Student"))
    {
        await _userManager.AddToRoleAsync(user, "Student");
    }

    return Content("User is now a Student.");
    }

}
