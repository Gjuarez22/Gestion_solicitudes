using System;
using System.Collections.Generic;

namespace GestionSolicitud.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string? Usuario1 { get; set; }

    public string? Clave { get; set; }

    public string? Nomnbre { get; set; }

    public int? Idrol { get; set; }

    public virtual Rol? IdrolNavigation { get; set; }
}
