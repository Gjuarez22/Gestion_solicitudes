using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace GestionSolicitud.Models;

public partial class Flsolicitud
{
    public int IdSolicitud { get; set; }

    public DateTime? Fecha { get; set; }

    public int? IdSolicitante { get; set; }

    public int? IdTipoSolicitud { get; set; }

    public int? IdArea { get; set; }

    public string? IdStatus { get; set; }

    public string? DocNumErp { get; set; }

    public string? Comentarios { get; set; }

    public bool? Cancelada { get; set; }

    public bool? Reenviada { get; set; }

    public virtual ICollection<FlflujoProceso> FlflujoProcesos { get; set; } = new List<FlflujoProceso>();

    public virtual ICollection<FlsolicitudDet> FlsolicitudDets { get; set; } = new List<FlsolicitudDet>();

    public virtual Flarea? IdAreaNavigation { get; set; }

    public virtual Flusuario? IdSolicitanteNavigation { get; set; }

    public virtual Flstatus? IdStatusNavigation { get; set; }

    public virtual FltipoSolicitud? IdTipoSolicitudNavigation { get; set; }

   
}
