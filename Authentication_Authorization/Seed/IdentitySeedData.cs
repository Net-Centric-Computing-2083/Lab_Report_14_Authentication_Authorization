using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Authentication_Authorization.Seed
{
    public static class IdentitySeedData
    {
        public static async Task InitializeAsync(
            IServiceProvider serviceProvider)
        {
            var roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager =
                serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // Create roles
            string[] roles = { "Admin", "Student" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole(role));
                }
            }

            // Create Admin user
            var adminEmail = "admin@gmail.com";
            var adminPassword = "Admin123";

            var adminUser =
                await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(
                    adminUser,
                    adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(
                        adminUser,
                        "Admin");
                }
            }

            // Create Student user
            var studentEmail = "student@gmail.com";
            var studentPassword = "Student123";

            var studentUser =
                await userManager.FindByEmailAsync(studentEmail);

            if (studentUser == null)
            {
                studentUser = new IdentityUser
                {
                    UserName = studentEmail,
                    Email = studentEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(
                    studentUser,
                    studentPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(
                        studentUser,
                        "Student");

                    await userManager.AddClaimAsync(
                        studentUser,
                        new Claim("Department", "CSIT"));
                }
            }
        }
    }
}