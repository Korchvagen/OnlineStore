using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using OnlineStore;
using OnlineStore.DAL.SeedData;
using OnlineStore.DAL.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Host.UseNLog();

builder.Services.AddDbContext<OnlineStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OnlineStoreContext") ?? throw new InvalidOperationException("Connection string 'OnlineStoreContext' not found.")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = "/Accounts/LogIn");
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.RepositoriesInitializer();
builder.Services.ServicesInitializer();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    OnlineStoreInitializer.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

app.Run();
