namespace ECommerce.Application.Dtos
{
    public class InventoryDto
    {
        public int? ProductVariantId { get; set; }
        public int WarehouseId { get; set; }
        public int Quantity { get; set; }
    }

    public class CreateInventoryDto
    {
        public int ProductVariantId { get; set; }
        public int WarehouseId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateInventoryDto
    {
        public int Quantity { get; set; }
    }
}