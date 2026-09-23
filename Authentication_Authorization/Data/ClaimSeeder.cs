using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace SecurityDemo.Data
{
    public static class ClaimSeeder
    {
        public static async Task SeedClaimsAsync(
            UserManager<IdentityUser> userManager)
        {
            var user =
                await userManager.FindByEmailAsync(
                    "student@gmail.com");

            if (user == null)
                return;

            var claims =
                await userManager.GetClaimsAsync(user);

            if (!claims.Any(c =>
                c.Type == "Department" &&
                c.Value == "CSIT"))
            {
                await userManager.AddClaimAsync(
                    user,
                    new Claim(
                        "Department",
                        "CSIT"));
            }
        }
    }
}