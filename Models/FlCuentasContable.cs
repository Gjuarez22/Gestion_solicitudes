using System;
using System.Collections.Generic;

namespace GestionSolicitud.Models;

public partial class FlCuentasContable
{
    public int Id { get; set; }

    public string? ItemCode { get; set; }

    public string? Cuenta { get; set; }
}
