using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SecurityDemo.Controllers
{
    public class AccountManagementController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AccountManagementController(
            UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> MakeAdmin(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return NotFound("User not found.");

            if (!await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }

            return Content($"{email} is now an Admin.");
        }

        public async Task<IActionResult> MakeStudent(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return NotFound("User not found.");

            if (!await _userManager.IsInRoleAsync(user, "Student"))
            {
                await _userManager.AddToRoleAsync(user, "Student");
            }

            return Content($"{email} is now a Student.");
        }
        public async Task<IActionResult> AddCSITClaim(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return NotFound("User not found.");

            var existingClaims = await _userManager.GetClaimsAsync(user);

            if (!existingClaims.Any(c =>
                c.Type == "Department" && c.Value == "CSIT"))
            {
                await _userManager.AddClaimAsync(
                    user,
                    new Claim("Department", "CSIT"));
            }

            return Content(
                $"{email} has been assigned Department = CSIT.");
        }
    }
}