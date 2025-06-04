namespace GestionSolicitud.Models
{
    public class SpListadoSoliciturdes
    {
        public int IdSolicitud { get; set; }
        public DateTime Creado { get; set; }
        public string DocNum { get; set; }
        public string Comentarios { get; set; }
        public string Tipo_Solicitud { get; set; }
        public string Nombre { get; set; }
        public string NombreArea { get; set; }
        public string NombreStatus { get; set; }
    }
}
