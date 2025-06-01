using System;
using System.Collections.Generic;

namespace GestionSolicitud.Models;

public partial class VwProducto
{
    public string Codigo { get; set; } = null!;

    public string? ItemName { get; set; }

    public string ItmsGrpNam { get; set; } = null!;

    public string? WhsCode { get; set; }

    public string? UniMed { get; set; }

    public decimal? Existencia { get; set; }

    public string? AreaRepuesto { get; set; }

    public string? AcctCode { get; set; }
}
