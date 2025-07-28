using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionSolicitud.Models;

public partial class FlsolicitudDet
{
    public int IdSolicitud { get; set; }

    public string IdProducto { get; set; } = null!;

    public decimal? Cantidad { get; set; }

    public int IdMaquina { get; set; }

    public string? Umedida { get; set; }

    public string? ComentariosDet { get; set; }

    public virtual Flmaquina IdMaquinaNavigation { get; set; } = null!;

    public virtual Flsolicitud IdSolicitudNavigation { get; set; } = null!;

    [NotMapped]
    public string? descripcionProducto { get; set; }

    [NotMapped]
    public string? UbicacionProducto { get; set; }
}
