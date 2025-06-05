using System.ComponentModel.DataAnnotations;

namespace GestionSolicitud.ViewModels
{
    public class IngresoViewModel
    {
        [Required(ErrorMessage = "El email es requerido")]
        //[EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string usuario { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }

    }
}
