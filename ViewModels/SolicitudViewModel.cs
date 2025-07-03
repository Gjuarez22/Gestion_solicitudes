using GestionSolicitud.Models;
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


        [Display(Name = "Comentarios")]
        public string? Comentarios { get; set; }

        [Display(Name = "Detalle")]
        [Required(ErrorMessage = "Ingrese al menos un detalle")]
        public List<DetalleSolicitudLinea> detalle { get; set; }
       
        // SelectLists
        public SelectList? Areas { get; set; }
        public SelectList? Solicitantes { get; set; }
        public List<TipoSolicitudViewModel>? TiposSolicitud { get; set; }
        public SelectList? Estados { get; set; }
        public SelectList? Maquinas { get; set; }

      
    }

    public class TipoSolicitudViewModel
    {
        public TipoSolicitudViewModel(){}
        public TipoSolicitudViewModel(FltipoSolicitud solicitud,bool maquina){
            id = solicitud.IdTipoSolicitud;
            nombre = solicitud.NombreTipoSolicitud;

           mostrarMaquina = maquina;
        }
        public int id {  get; set; }
        public string nombre { get; set; }
        public bool mostrarMaquina {  get; set; }
    }

    public class DetalleSolicitudLinea
    {
        public int id { get; set; }
        public int idProducto { get; set; }
        public int cantidad { get; set; }
        public int idMaquina { get; set; }
        public string? nombreProducto { get; set; }
        public int? maximoCantidad { get; set; }
    }
}
