using System;
using System.Collections.Generic;

namespace GestionSolicitud.Models;

public partial class FltipoSxSolicitante
{
    public int IdTipoSxSolicitante { get; set; }

    public int IdTipoSolicitud { get; set; }

    public int IdSolicitante { get; set; }

    public virtual Flusuario IdSolicitanteNavigation { get; set; } = null!;

    public virtual FltipoSolicitud IdTipoSolicitudNavigation { get; set; } = null!;
}
