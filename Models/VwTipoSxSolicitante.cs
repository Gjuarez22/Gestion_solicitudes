using System;
using System.Collections.Generic;

namespace GestionSolicitud.Models;

public partial class VwTipoSxSolicitante
{
    public int IdTipoSolicitud { get; set; }

    public string? NombreTipoSolicitud { get; set; }

    public int? Bodega { get; set; }

    public int? IdFlujo { get; set; }

    public bool? ActivoTipoS { get; set; }

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
