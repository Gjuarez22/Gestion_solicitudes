using GestionSolicitud.Models;
using GestionSolicitud.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using GestionSolicitud.DTESupplier.Logica;
using GestionSolicitud.HelperClasses;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DbFlujosTestContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("bdLocal")));

//Servicios
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<SapGoodsIssueCreator>();
builder.Services.AddScoped<MiServicio>();


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
        options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
        options.SlidingExpiration = true;

        //ESTE CÓDIGO evita que redireccione cuando es llamada por AJAX / API
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                var path = context.Request.Path;

                // Aquí especificas rutas que deben permitirse
                if (path.StartsWithSegments("/Autorizacion/SetAuthorization", StringComparison.OrdinalIgnoreCase))
                {
                    context.Response.StatusCode = 200; // o 403 si prefieres
                    return Task.CompletedTask;
                }

                context.Response.Redirect(context.RedirectUri);
                return Task.CompletedTask;
            }
        };


    });


// Agregar MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();



//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

if (!app.Environment.IsDevelopment())
{
    // ✅ Muestra error directo en navegador (más útil para depuración)
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("⚠️ Error interno en el servidor.");
        });
    });

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


