using System;
using System.Collections.Generic;

namespace GestionSolicitud.Models;

public partial class VwEmpleado
{
    public string ExpCodigoAlternativo { get; set; } = null!;

    public string? Nombre { get; set; }

    public string? Usuario { get; set; }

    public int? CodDepto { get; set; }

    public string? NomDepto { get; set; }

    public string? ExpEmail { get; set; }

    public string? EmpEstado { get; set; }
}
