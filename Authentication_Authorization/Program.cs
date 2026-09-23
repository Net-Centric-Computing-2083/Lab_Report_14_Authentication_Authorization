using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SecurityDemo.Data;

var builder = WebApplication.CreateBuilder(args);

// ======================================================
// 1. Configure Database
// ======================================================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));


// ======================================================
// 2. Configure ASP.NET Core Identity
// ======================================================

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    // Email confirmation is not required for this lab
    options.SignIn.RequireConfirmedAccount = false;

    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();


// ======================================================
// 3. Configure Authorization Policies
// ======================================================

builder.Services.AddAuthorization(options =>
{
    // Only users having Department = CSIT
    // can access resources protected by this policy
    options.AddPolicy("CSITOnly", policy =>
    {
        policy.RequireClaim("Department", "CSIT");
    });
});


// ======================================================
// 4. Add MVC Services
// ======================================================

builder.Services.AddControllersWithViews();


// ======================================================
// Build Application
// ======================================================

var app = builder.Build();


// ======================================================
// 5. Configure HTTP Request Pipeline
// ======================================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();


// ======================================================
// 6. Authentication and Authorization Middleware
// ======================================================

app.UseAuthentication();

app.UseAuthorization();


// ======================================================
// 7. Configure MVC Routing
// ======================================================

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// ======================================================
// 8. Configure Identity Razor Pages
// ======================================================

app.MapRazorPages();


// ======================================================
// 9. Seed Roles
// ======================================================

using (var scope = app.Services.CreateScope())
{
    var roleManager =
        scope.ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();

    await SeedData.SeedRolesAsync(roleManager);
}


// ======================================================
// 10. Seed Admin User and Assign Admin Role
// ======================================================

using (var scope = app.Services.CreateScope())
{
    var userManager =
        scope.ServiceProvider
            .GetRequiredService<UserManager<IdentityUser>>();

    await UserRoleSeeder.SeedAdminAsync(userManager);
}


// ======================================================
// 11. Seed Student User and Assign Student Role
// ======================================================

using (var scope = app.Services.CreateScope())
{
    var userManager =
        scope.ServiceProvider
            .GetRequiredService<UserManager<IdentityUser>>();

    await StudentRoleSeeder.SeedStudentAsync(userManager);
}


// ======================================================
// 12. Seed Department Claim
// ======================================================

using (var scope = app.Services.CreateScope())
{
    var userManager =
        scope.ServiceProvider
            .GetRequiredService<UserManager<IdentityUser>>();

    await ClaimSeeder.SeedClaimsAsync(userManager);
}


// ======================================================
// Run Application
// ======================================================

app.Run();