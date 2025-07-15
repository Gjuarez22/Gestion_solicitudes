namespace GestionSolicitud.HelperClasses
{
    public class InventoryTransferItem
    {
        public string ItemCode { get; set; }

        public double Quantity { get; set; }

        public string FromWarehouse { get; set; }

        public string ToWarehouse { get; set; }
    }

}
