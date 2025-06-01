using System;
using System.Collections.Generic;

namespace GestionSolicitud.Models;

public partial class FlflujoProceso
{
    public int IdSolicitud { get; set; }

    public int? IdFlujo { get; set; }

    public int IdFlujoDet { get; set; }

    public int? IdAutorizador { get; set; }

    public int? IdAutorizadorAlterno { get; set; }

    public int? IdEncargadoBodega { get; set; }

    public int? Paso { get; set; }

    public string? SiguienteEstado { get; set; }

    public bool? Ejecutado { get; set; }

    public bool? Autorizado { get; set; }

    public virtual Flusuario? IdAutorizadorAlternoNavigation { get; set; }

    public virtual Flusuario? IdAutorizadorNavigation { get; set; }

    public virtual FlflujoDet IdFlujoDetNavigation { get; set; } = null!;

    public virtual Flsolicitud IdSolicitudNavigation { get; set; } = null!;
}
