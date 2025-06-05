using GestionSolicitud.Models;
using GestionSolicitud.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DbFlujosTestContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("bdLocal")));

//Servicios
builder.Services.AddScoped<IAuthService, AuthService>();

// Agrega esto antes de builder.Build():
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation(); 

// Configurar autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Ingreso/Login";
        options.LogoutPath = "/Ingreso/Logout";
        options.AccessDeniedPath = "/Ingreso/AccesoDenegado";
        options.ExpireTimeSpan = TimeSpan.FromHours(24);
        options.SlidingExpiration = true;
    });

// Agregar MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();



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
    pattern: "{controller=Ingreso}/{action=Login}/{id?}");

app.Run();
