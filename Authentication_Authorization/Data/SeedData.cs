using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Authentication_Authorization.Data
{
    public static class SeedData
    {
        public static async Task Initialize(
            IServiceProvider serviceProvider)
        {
            var roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager =
                serviceProvider.GetRequiredService<UserManager<IdentityUser>>();


            // ==================================================
            // CREATE ROLES
            // ==================================================

            string[] roles =
            {
                "Admin",
                "Student"
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole(role));
                }
            }


            // ==================================================
            // CREATE ADMIN USER
            // ==================================================

            string adminEmail = "admin@gmail.com";
            string adminPassword = "Admin@123";


            var admin =
                await userManager.FindByEmailAsync(adminEmail);


            if (admin == null)
            {
                admin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result =
                    await userManager.CreateAsync(
                        admin,
                        adminPassword);

                if (!result.Succeeded)
                {
                    throw new Exception(
                        "Admin user creation failed.");
                }
            }


            // Add Admin role

            if (!await userManager.IsInRoleAsync(
                admin,
                "Admin"))
            {
                await userManager.AddToRoleAsync(
                    admin,
                    "Admin");
            }


            // Add CSIT claim

            var adminClaims =
                await userManager.GetClaimsAsync(admin);


            if (!adminClaims.Any(c =>
                c.Type == "Department" &&
                c.Value == "CSIT"))
            {
                await userManager.AddClaimAsync(
                    admin,
                    new Claim(
                        "Department",
                        "CSIT"));
            }


            // ==================================================
            // CREATE STUDENT USER
            // ==================================================

            string studentEmail = "student@gmail.com";
            string studentPassword = "Student@123";


            var student =
                await userManager.FindByEmailAsync(studentEmail);


            if (student == null)
            {
                student = new IdentityUser
                {
                    UserName = studentEmail,
                    Email = studentEmail,
                    EmailConfirmed = true
                };

                var result =
                    await userManager.CreateAsync(
                        student,
                        studentPassword);

                if (!result.Succeeded)
                {
                    throw new Exception(
                        "Student user creation failed.");
                }
            }


            // Add Student role

            if (!await userManager.IsInRoleAsync(
                student,
                "Student"))
            {
                await userManager.AddToRoleAsync(
                    student,
                    "Student");
            }


            // Add CSIT claim

            var studentClaims =
                await userManager.GetClaimsAsync(student);


            if (!studentClaims.Any(c =>
                c.Type == "Department" &&
                c.Value == "CSIT"))
            {
                await userManager.AddClaimAsync(
                    student,
                    new Claim(
                        "Department",
                        "CSIT"));
            }
        }
    }
}