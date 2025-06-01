﻿using System;
using System.Collections.Generic;

namespace GestionSolicitud.Models;

public partial class VwSolicitudEncabezado
{
    public int IdSolicitud { get; set; }

    public DateTime? Fecha { get; set; }

    public int? IdSolicitante { get; set; }

    public int? IdTipoSolicitud { get; set; }

    public int? IdArea { get; set; }

    public string? IdStatus { get; set; }

    public string? DocNumErp { get; set; }

    public string? Comentarios { get; set; }

    public bool? Cancelada { get; set; }

    public string? NombreStatus { get; set; }

    public string? NombreArea { get; set; }

    public string? NombreTipoSolicitud { get; set; }

    public int? Bodega { get; set; }

    public string? Nombre { get; set; }

    public string? CodEmplea { get; set; }

    public string? Email { get; set; }

    public string? Usuario { get; set; }

    public string? Unidad { get; set; }

    public int? IdEncargadoBodega { get; set; }
}
