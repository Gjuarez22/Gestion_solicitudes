using System;
using System.Collections.Generic;

namespace GestionSolicitud.Models;

public partial class VwFlujoDet
{
    public int IdSolicitud { get; set; }

    public DateTime? Fecha { get; set; }

    public int? IdSolicitante { get; set; }

    public int? IdTipoSolicitud { get; set; }

    public int? IdArea { get; set; }

    public string? IdStatus { get; set; }

    public string? NombreTipoSolicitud { get; set; }

    public int? Bodega { get; set; }

    public int? IdFlujo { get; set; }

    public string? DescripcionFlujo { get; set; }

    public decimal? HorasReintento { get; set; }

    public int IdFlujoDet { get; set; }

    public int IdAutorizador { get; set; }

    public int IdAutorizadorAlterno { get; set; }

    public int? IdEncargadoBodega { get; set; }

    public int? Paso { get; set; }

    public string? StatusSiguiente { get; set; }

    public bool? Cancelada { get; set; }

    public string? DocNumErp { get; set; }

    public string? Comentarios { get; set; }
}
