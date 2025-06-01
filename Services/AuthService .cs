using GestionSolicitud.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace GestionSolicitud.Services
{
    public class AuthService : IAuthService
    {
        private readonly DbFlujosTestContext _context;

        public AuthService(DbFlujosTestContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, string Message, Flusuario Usuario, List<string> Roles)> ValidateUserAsync(string email, string password)
        {
            try
            {
                // Buscar usuario por email
                var usuario = await _context.Flusuarios
                    .Where(x => x.Email == email)
                    .Include(u => u.IdRols)
                    .FirstOrDefaultAsync();

                if (usuario == null)
                {
                    return (false, "Usuario no encontrado o inactivo", null, null);
                }

                // Verificar contraseña
                if (!VerifyPassword(password, usuario.Contrasena))
                {
                    return (false, "Contraseña incorrecta", null, null);
                }

                // Obtener roles del usuario
                var roles = usuario.IdRols.Select(r=>r.NombreRol).ToList();

                return (true, "Login exitoso", usuario, roles);

            }
            catch (Exception ex)
            {
                return (false, $"Error en el login: {ex.Message}", null, null);
            }
        }

        public string HashPassword(string password)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] salt = new byte[16];
                rng.GetBytes(salt);

                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
                byte[] hash = pbkdf2.GetBytes(20);

                byte[] hashBytes = new byte[36];
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);

                return Convert.ToBase64String(hashBytes);
            }
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            /*byte[] hashBytes = Convert.FromBase64String(hashedPassword);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                    return false;
            }*/
            if(password == hashedPassword)
            { return true; }
            else
            {
                return false;
            }
        }
    }
}
