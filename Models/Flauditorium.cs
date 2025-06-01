using System;
using System.Collections.Generic;

namespace GestionSolicitud.Models;

public partial class Flauditorium
{
    public int IdAuditoria { get; set; }

    public int? IdSolicitud { get; set; }

    public string? IdStatus { get; set; }

    public DateTime? Fecha { get; set; }

    public int? IdUsuario { get; set; }

    public string? Notas { get; set; }
}
