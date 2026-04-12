using Honeytor.Data;
using Honeytor.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static System.Formats.Asn1.AsnWriter;

static async Task Matungo_SeedRolesAsync
(
    RoleManager<ApplicationRole> roleManager
)
{
    int level = 0;
    string[] roles = {"User", "Mederator", "Admin" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new ApplicationRole{ Name = role, PrivilegeLevel = level });

            level++;
        }   
    }
}
static async Task Matungo_SeedAdminAccountAsync
(
      UserManager<ApplicationUser> userManager,
      string adminEmail,
      string adminPassword
)
{
    var user = await userManager.FindByEmailAsync(adminEmail);

    if (user == null)
    {
        user = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, adminPassword);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
        else
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

var scope = app.Services.CreateScope();

/* AH AH ROLY */
await Matungo_SeedRolesAsync(scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>());
await Matungo_SeedAdminAccountAsync(scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>(),
    "admin@leka.nosht",
    "admin1A."
);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
