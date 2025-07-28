using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionSolicitud.Models;

public partial class Flauditorium
{
    public int IdAuditoria { get; set; }

    public int? IdSolicitud { get; set; }

    public string? IdStatus { get; set; }

    public DateTime? Fecha { get; set; }

    public int? IdUsuario { get; set; }

    public string? Notas { get; set; }

    [NotMapped]
    public string? Estatus { get; set; }

    [NotMapped]
    public string? Ususario_ { get; set; }

    ///public virtual ICollection<Flstatus> Flstatuses { get; set; } = new List<Flstatus>();
    [ForeignKey(nameof(IdStatus))]
    public virtual Flstatus? StatusNavigation { get; set; }



}
