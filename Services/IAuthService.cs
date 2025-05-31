using GestionSolicitud.Models;

namespace GestionSolicitud.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, Usuario Usuario, List<string> Roles)> ValidateUserAsync(string email, string password);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
