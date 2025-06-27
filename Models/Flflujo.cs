using System;
using System.Collections.Generic;

namespace GestionSolicitud.Models;

public partial class Flflujo
{
    public int IdFlujo { get; set; }

    public string? DescripcionFlujo { get; set; }

    public decimal? HorasReintento { get; set; }

    public bool? Activo { get; set; }

    public int? SeeMaquina { get; set; }

    public virtual ICollection<FlflujoDet> FlflujoDets { get; set; } = new List<FlflujoDet>();
}
