using Microsoft.AspNetCore.Identity;

namespace SecurityDemo.Data
{
    public static class StudentRoleSeeder
    {
        public static async Task SeedStudentAsync(
            UserManager<IdentityUser> userManager)
        {
            string email = "student@gmail.com";
            string password = "Student@123";

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
                user, "Student"))
            {
                await userManager.AddToRoleAsync(
                    user,
                    "Student");
            }
        }
    }
}