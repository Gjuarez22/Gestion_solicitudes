using System;
using System.Collections.Generic;

namespace GestionSolicitud.Models;

public partial class Flstatus
{
    public string IdStatus { get; set; } = null!;

    public string? NombreStatus { get; set; }

    public int? Orden { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<FlflujoDet> FlflujoDets { get; set; } = new List<FlflujoDet>();

    public virtual ICollection<Flsolicitud> Flsolicituds { get; set; } = new List<Flsolicitud>();
}
