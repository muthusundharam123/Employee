using Employee.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var employeeConnectionString = builder.Configuration.GetConnectionString("EmployeeConnectionString") ?? throw new InvalidOperationException("Connection string 'EmployeeConnectionString' not found.");
var identityConnectionString = builder.Configuration.GetConnectionString("IdentityConnectionString") ?? throw new InvalidOperationException("Connection string 'IdentityConnectionString' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(identityConnectionString));

builder.Services.AddDbContext<EmployeeDbContext>(options =>
    options.UseSqlServer(employeeConnectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
});


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
    pattern: "{controller=Employee}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
