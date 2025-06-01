using System;
using System.Collections.Generic;

namespace GestionSolicitud.Models;

public partial class Flmaquina
{
    public int IdMaquina { get; set; }

    public string? NombreMaquina { get; set; }

    public string? CodigoReferencia { get; set; }

    public virtual ICollection<FlsolicitudDet> FlsolicitudDets { get; set; } = new List<FlsolicitudDet>();
}
