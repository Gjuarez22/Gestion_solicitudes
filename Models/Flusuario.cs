using System;
using System.Collections.Generic;

namespace GestionSolicitud.Models;

public partial class Flusuario
{
    public int IdUsuario { get; set; }

    public string? Nombre { get; set; }

    public string? CodEmplea { get; set; }

    public string? Usuario { get; set; }

    public string? Email { get; set; }

    public string? Contrasena { get; set; }

    public string? Unidad { get; set; }

    public bool? Activo { get; set; }

    public DateTime? Fecharegistro { get; set; }

    public virtual ICollection<FlflujoDet> FlflujoDetIdAutorizadorAlternoNavigations { get; set; } = new List<FlflujoDet>();

    public virtual ICollection<FlflujoDet> FlflujoDetIdAutorizadorNavigations { get; set; } = new List<FlflujoDet>();

    public virtual ICollection<FlflujoProceso> FlflujoProcesoIdAutorizadorAlternoNavigations { get; set; } = new List<FlflujoProceso>();

    public virtual ICollection<FlflujoProceso> FlflujoProcesoIdAutorizadorNavigations { get; set; } = new List<FlflujoProceso>();

    public virtual ICollection<Flsolicitud> Flsolicituds { get; set; } = new List<Flsolicitud>();

    public virtual ICollection<FltipoSxSolicitante> FltipoSxSolicitantes { get; set; } = new List<FltipoSxSolicitante>();

    public virtual ICollection<Flrol> IdRols { get; set; } = new List<Flrol>();
}
