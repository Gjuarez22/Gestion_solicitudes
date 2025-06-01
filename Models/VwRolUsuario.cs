using System;
using System.Collections.Generic;

namespace GestionSolicitud.Models;

public partial class VwRolUsuario
{
    public int IdRol { get; set; }

    public string? NombreRol { get; set; }

    public int IdUsuario { get; set; }

    public string? Nombre { get; set; }

    public string? CodEmplea { get; set; }

    public string? Usuario { get; set; }

    public string? Email { get; set; }

    public string? Contrasena { get; set; }

    public string? Unidad { get; set; }

    public bool? Activo { get; set; }

    public DateTime? Fecharegistro { get; set; }
}
