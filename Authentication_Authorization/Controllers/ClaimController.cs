using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SecurityDemo.Controllers
{
    [Authorize]
    public class ClaimController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ClaimController(
            UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> AddDepartmentClaim()
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
                    new Claim("Department", "CSIT")
                );
            }

            return Content(
                "Department claim CSIT assigned successfully."
            );
        }

        public IActionResult Department()
        {
            var department =
                User.FindFirst("Department")?.Value;

            ViewBag.Department =
                department ?? "Department claim not found.";

            return View();
        }
    }
}