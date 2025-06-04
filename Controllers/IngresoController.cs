using GestionSolicitud.Services;
using GestionSolicitud.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GestionSolicitud.Controllers
{
    public class IngresoController : Controller
    {
        //Heredamos el servicio de autenticación previamente definido
        private readonly IAuthService _authService;
        public IngresoController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(IngresoViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var result = await _authService.ValidateUserAsync(model.usuario, model.Password);

                if (result.Success)
                {
                    //Agrega los valores a las cookies, nombre, email, id
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, result.Usuario.IdUsuario.ToString()),
                        new Claim(ClaimTypes.Name, result.Usuario.Nombre),
                        new Claim(ClaimTypes.Email, result.Usuario.Email),
                        new Claim(ClaimTypes.Role, result.Usuario.IdRols.Select(r=>r.IdRol).FirstOrDefault().ToString()),

                    };

                    // Agregar roles 
                    foreach (var role in result.Roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24) //Tiempo de expiracion de la cookie
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), authProperties);

                    //Retorna la vista a la vista de inicio
                    ViewBag.inicioSesion = true;
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message); //Retorna los errores a la vista
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccesoDenegado()
        {
            return View();
        }
    }
}
