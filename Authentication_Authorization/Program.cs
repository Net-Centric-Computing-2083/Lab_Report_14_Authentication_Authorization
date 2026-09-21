using Authentication_Authorization.Data;
using Authentication_Authorization.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add database context using SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure ASP.NET Core Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;

    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

// Configure authorization policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CSITOnly", policy =>
    {
        policy.RequireClaim("Department", "CSIT");
    });
});

// Add MVC and Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// Authentication must come before Authorization
app.UseAuthentication();

app.UseAuthorization();

// Seed roles, users, and claims
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await IdentitySeedData.InitializeAsync(services);
}

// Configure routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();