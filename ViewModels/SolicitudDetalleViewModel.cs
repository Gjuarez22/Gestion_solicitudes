using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GestionSolicitud.ViewModels
{
    public class SolicitudDetalleViewModel
    {
        public int IdSolicitud { get; set; }

        [Display(Name = "Solicitante")]
        public int? IdSolicitante { get; set; }


        [Display(Name = "Maquina")]
        [Required(ErrorMessage = "La Maquina es obligatoria")]
        public int? IdMaquina { get; set; }

        [Display(Name = "Producto")]
        [Required(ErrorMessage = "Seleccion de Producto es obligatoria")]
        public int? IdProducto { get; set; }

        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "La Cantidad es obligatoria")]
        public int? Cantidad { get; set; }


        // SelectLists
        public SelectList? Producto { get; set; }
        public SelectList? Maquina { get; set; }
    
    }
}
