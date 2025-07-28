
using System;
using System.Collections.Generic;
using GestionSolicitud.HelperClasses;

namespace GestionSolicitud.HelperClasses
{
    public class InventoryTransferRequest
    {
        public string CardCode { get; set; }

        public DateTime DocDate { get; set; }

        public DateTime TaxDate { get; set; }

        public DateTime DocDueDate { get; set; }

        public string Comments { get; set; }

        public List<InventoryTransferItem> Items { get; set; }
    }


}
