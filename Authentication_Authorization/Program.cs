using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Authentication_Authorization.Data;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------
// DATABASE
// ----------------------------------------------------

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=securitydemo.db";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));


// ----------------------------------------------------
// ASP.NET CORE IDENTITY
// ----------------------------------------------------

builder.Services
    .AddDefaultIdentity<IdentityUser>(options =>
    {
        // Email confirmation is disabled for this practical
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


// ----------------------------------------------------
// AUTHORIZATION
// ----------------------------------------------------

builder.Services.AddAuthorization(options =>
{
    // CSIT Policy
    options.AddPolicy("CSITOnly", policy =>
    {
        policy.RequireClaim("Department", "CSIT");
    });
});


// ----------------------------------------------------
// MVC
// ----------------------------------------------------

builder.Services.AddControllersWithViews();


var app = builder.Build();


// ----------------------------------------------------
// HTTP REQUEST PIPELINE
// ----------------------------------------------------

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();


// IMPORTANT
// Authentication must come before Authorization

app.UseAuthentication();

app.UseAuthorization();


// ----------------------------------------------------
// MVC ROUTING
// ----------------------------------------------------

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// ----------------------------------------------------
// IDENTITY RAZOR PAGES
// ----------------------------------------------------

app.MapRazorPages();


// ----------------------------------------------------
// CREATE ROLES, USERS AND CLAIMS
// ----------------------------------------------------

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await SeedData.Initialize(services);
}


app.Run();