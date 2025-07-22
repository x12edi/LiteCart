﻿namespace ECommerce.Domain.Entities
{
    public class StockMovement
    {
        public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; }
        public int? FromWarehouseId { get; set; }
        public Warehouse FromWarehouse { get; set; }
        public int? ToWarehouseId { get; set; }
        public Warehouse ToWarehouse { get; set; }
        public int Quantity { get; set; }
        public string Reason { get; set; } // e.g., Restock, Transfer, Return
        public DateTime Timestamp { get; set; }
    }
}