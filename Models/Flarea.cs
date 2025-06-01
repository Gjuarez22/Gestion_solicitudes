using System;
using System.Collections.Generic;

namespace GestionSolicitud.Models;

public partial class Flarea
{
    public int IdArea { get; set; }

    public string? NombreArea { get; set; }

    public virtual ICollection<Flsolicitud> Flsolicituds { get; set; } = new List<Flsolicitud>();
}
