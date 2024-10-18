using ImaadProgP2.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add Identity with roles
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
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

// Seed roles and users on startup
await SeedDataAsync(app);

app.Run();

// Seed roles and users
async Task SeedDataAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

        await SeedRolesAsync(roleManager);
        await SeedUsersAsync(userManager);
    }
    catch (Exception ex)
    {
        // Log the error (uncomment ex variable name and write a log using a logging framework)
        // logger.LogError(ex, "An error occurred seeding the database.");
    }
}

// Seed roles
async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
{
    string[] roleNames = { "Lecturer", "Admin", "Programme Coordinator", "Academic Manager" };

    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

// Seed users with roles
async Task SeedUsersAsync(UserManager<IdentityUser> userManager)
{
    var users = new (string Email, string Password, string Role)[]
    {
        ("lect@gmail.com", "Lect123", "Lecturer"),
        ("admin@admin.com", "Admin@123", "Admin"),
        ("Academ@gmail.com", "ACMan@123", "Academic Manager"),
        ("prCo@gmail.com", "ProCon123", "Programme Coordinator")
    };

    foreach (var (email, password, role) in users)
    {
        if (userManager.Users.All(u => u.Email != email))
        {
            var user = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}
