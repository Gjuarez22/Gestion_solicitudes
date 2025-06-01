using System;
using System.Collections.Generic;

namespace GestionSolicitud.Models;

public partial class Flrol
{
    public int IdRol { get; set; }

    public string? NombreRol { get; set; }

    public virtual ICollection<Flusuario> IdUsuarios { get; set; } = new List<Flusuario>();
}
