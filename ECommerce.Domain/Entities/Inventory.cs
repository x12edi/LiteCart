namespace ECommerce.Domain.Entities
{
    public class Inventory
    {
        public int ProductId { get; set; }
        public int? VariantId { get; set; } // Nullable for product-level inventory
        public Product Product { get; set; }
        public ProductVariant Variant { get; set; }
        public int Quantity { get; set; }
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}