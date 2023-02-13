using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WestCoastEducation.web.Data;

var builder = WebApplication.CreateBuilder(args);

// Add database support
builder.Services.AddDbContext<WestCoastEducationContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"))
);

//Identity setup
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.User.RequireUniqueEmail = true;

}).AddEntityFrameworkStores<WestCoastEducationContext>();

//Cookie setup
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/account/login";
});

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed the database
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
