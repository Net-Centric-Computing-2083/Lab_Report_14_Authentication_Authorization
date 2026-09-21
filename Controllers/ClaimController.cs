using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lab14.Controllers;

[Authorize]
public class ClaimController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;

    public ClaimController(
        UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> AddDepartment()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return Unauthorized();
        }

        var existingClaims =
            await _userManager.GetClaimsAsync(user);

        if (!existingClaims.Any(c =>
            c.Type == "Department"))
        {
            await _userManager.AddClaimAsync(
                user,
                new Claim("Department", "CSIT"));
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return Unauthorized();
        }

        var claims = await _userManager.GetClaimsAsync(user);

        var department = claims
            .FirstOrDefault(c => c.Type == "Department")
            ?.Value;

        ViewBag.Department = department;

        return View();
    }
}
