using System;
using System.Collections.Generic;

namespace GestionSolicitud.Models;

public partial class VwProcesarEnErp
{
    public int IdSolicitud { get; set; }

    public DateTime? Fecha { get; set; }

    public int? Bodega { get; set; }

    public string? Comentarios { get; set; }

    public string IdProducto { get; set; } = null!;

    public string? ItemName { get; set; }

    public decimal? Cantidad { get; set; }

    public string? Umedida { get; set; }

    public string? ComentariosDet { get; set; }

    public string? IdStatus { get; set; }

    public string? Usuario { get; set; }

    public string? NombreArea { get; set; }
}
