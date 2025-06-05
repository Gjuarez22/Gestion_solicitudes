namespace GestionSolicitud.Models
{
    public class SpListadoSoliciturdes
    {
        public int Id { get; set; }
        public string tipoDeSolicitud { get; set; }
        public DateTime Creada { get; set; }
        public string DocNum { get; set; }
        public string Comentarios { get; set; }
        public string Solicitante { get; set; }
        public string Area { get; set; }
        public string Estado{ get; set; }
    }
}
