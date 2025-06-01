using System;
using System.Collections.Generic;

namespace GestionSolicitud.Models;

public partial class VwEmailsNotificarAut
{
    public int IdSolicitud { get; set; }

    public string? EmailSolicitante { get; set; }

    public string? EmailEncargadoBodega { get; set; }
}
