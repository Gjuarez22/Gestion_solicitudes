using System;
using System.Collections.Generic;

namespace GestionSolicitud.Models;

public partial class FlflujoDet
{
    public int IdFlujoDet { get; set; }

    public int IdFlujo { get; set; }

    public int IdAutorizador { get; set; }

    public int IdAutorizadorAlterno { get; set; }

    public int? IdEncargadoBodega { get; set; }

    public int? Paso { get; set; }

    public string? StatusSiguiente { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<FlflujoProceso> FlflujoProcesos { get; set; } = new List<FlflujoProceso>();

    public virtual Flusuario IdAutorizadorAlternoNavigation { get; set; } = null!;

    public virtual Flusuario IdAutorizadorNavigation { get; set; } = null!;

    public virtual Flflujo IdFlujoNavigation { get; set; } = null!;

    public virtual Flstatus? StatusSiguienteNavigation { get; set; }
}
