using System;
using System.Collections.Generic;

namespace GestionSolicitud.Models;

public partial class FltipoSolicitud
{
    public int IdTipoSolicitud { get; set; }

    public string? NombreTipoSolicitud { get; set; }

    public int? Bodega { get; set; }

    public int? IdFlujo { get; set; }

    public bool? ActivoTipoS { get; set; }

    public virtual ICollection<Flsolicitud> Flsolicituds { get; set; } = new List<Flsolicitud>();

    public virtual ICollection<FltipoSxSolicitante> FltipoSxSolicitantes { get; set; } = new List<FltipoSxSolicitante>();
}
