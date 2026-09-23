using Microsoft.AspNetCore.Identity;

namespace SecurityDemo.Data
{
    public static class UserRoleSeeder
    {
        public static async Task SeedAdminAsync(
            UserManager<IdentityUser> userManager)
        {
            string email = "admin@gmail.com";
            string password = "Admin@123";

            var user =
                await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(
                    user,
                    password);
            }

            if (!await userManager.IsInRoleAsync(
                user, "Admin"))
            {
                await userManager.AddToRoleAsync(
                    user,
                    "Admin");
            }
        }
    }
}