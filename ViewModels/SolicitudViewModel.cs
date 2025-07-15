using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GestionSolicitud.ViewModels
{
    public class SolicitudViewModel
    {
        public int IdSolicitud { get; set; }

        [Display(Name = "Fecha")]
        public DateTime? Fecha { get; set; }

        [Display(Name = "Solicitante")]
        public int? IdSolicitante { get; set; }

        [Display(Name = "Tipo de Solicitud")]
        [Required(ErrorMessage = "El tipo de solicitud es obligatorio")]
        public int? IdTipoSolicitud { get; set; }

        [Display(Name = "Área")]
        [Required(ErrorMessage = "El área es obligatoria")]
        public int? IdArea { get; set; }

        //[Display(Name = "Estado")]
       // [Required(ErrorMessage = "El estado es obligatorio")]
        public string? IdStatus { get; set; }

       // [Display(Name = "Número Documento ERP")]
       // [Required(ErrorMessage = "El número de documento ERP es obligatorio")]
       // [StringLength(50, MinimumLength = 1, ErrorMessage = "El número de documento ERP debe tener entre 1 y 50 caracteres")]
        public string? DocNumErp { get; set; }

        [Display(Name = "Comentarios")]
        public string? Comentarios { get; set; }

        [Display(Name = "Cancelada")]
        public bool? Cancelada { get; set; }

        [Display(Name = "Reenviada")]
        public bool? Reenviada { get; set; }

        // SelectLists
        public SelectList? Areas { get; set; }
        public SelectList? Solicitantes { get; set; }
        public SelectList? TiposSolicitud { get; set; }
        public SelectList? Estados { get; set; }
        public SelectList? Maquinas { get; set; }

        
        //public SelectList?Productos { get; set; }

        //[Display(Name = "Productos")]
        //public string? Codigo { get; set; }
        
    }
}
